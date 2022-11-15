import json
import os
from pathlib import Path

from django.conf import settings
from django.shortcuts import render, redirect
from django.http import HttpResponse, HttpResponseRedirect, JsonResponse, FileResponse
from django.contrib.auth.decorators import login_required
from django.views.decorators.csrf import csrf_exempt
from django.utils.decorators import method_decorator


from .libs.execution_helpers import check_module, get_executions_helper, get_execution_helper
from .libs.hook_helpers import get_hooks_helper
from .libs.filecrawler_helpers import get_filecrawler_helper
from .libs.systeminfo_helpers import get_systeminfo_helper
from .libs.checksandbox_helpers import get_checksandbox_helper
from .libs.sharpedrchecker_helpers import get_sharpedrchecker_helper
from .libs.other_tools_helpers import get_alkhaser_helper, get_pafish_helper
from .libs.export_helper import export_json_helper
from .libs.helpers import b64decode
import logging

log_info = logging.getLogger('core.info')
log_error = logging.getLogger('core.error')

@login_required
def get_executions(request):
    """Get the list of executions
    """
    execution_l = get_executions_helper()
    context = {'execution_l': execution_l}
    return render(request, 'core/executions.html', context)

@login_required
def get_executions_json(request):
    execution_l = get_executions_helper()
    return JsonResponse(list(execution_l), safe=False)

@login_required
def get_execution(request, executionId):
    execution = get_execution_helper(executionId)
    sandbox = get_checksandbox_helper(execution.id)
    systeminfo = get_systeminfo_helper(execution.id)
    filecrawler = get_filecrawler_helper(execution.id)
    hooks = get_hooks_helper(execution.id)
    sharpedrchecker = get_sharpedrchecker_helper(execution.id)
    pafish = get_pafish_helper(execution.id)
    alkhaser = get_alkhaser_helper(execution.id)
    if len(sandbox) > 0:
        sandbox = sandbox[0]
    if len(systeminfo) > 0:
        systeminfo = systeminfo[0]
    if len(filecrawler) > 0:
        filecrawler = filecrawler[0]
    if len(hooks) > 0:
        hooks = hooks[0]
    if len(sharpedrchecker) > 0:
        sharpedrchecker = sharpedrchecker[0]
    pafish_aux = ''
    if len(pafish) > 0:
        pafish_aux = pafish[0]['pafish']
    alkhaser_aux = ''
    if len(alkhaser) > 0:
        alkhaser_aux = alkhaser[0]['alkhaser']
    context = {
        'execution': execution,
        'sandbox': sandbox,
        'systeminfo': systeminfo,
        'filecrawler': filecrawler,
        'hooks': hooks,
        'sharpedrchecker': sharpedrchecker,
        'pafish': pafish_aux,
        'alkhaser': alkhaser_aux,
        }
    return render(request, 'core/executionInfo.html', context)

@method_decorator(csrf_exempt, name='dispatch')
def collect_data(request):
    if request.method=='POST':
        try:
            received_json_data=json.loads(request.body)
            check_module(received_json_data, str(request.META['REMOTE_ADDR']))
            log_info.info(
                "collect_data -  IP: < " + str(request.META['REMOTE_ADDR']) + ">")
            print(f"[{b64decode(received_json_data['executionId'])}] Source: {request.META['REMOTE_ADDR']} -> module {b64decode(received_json_data['module'])}")
        except Exception as e:
            log_error.error(
                "collect_data -  IP: < " + str(request.META['REMOTE_ADDR']) + ">   -  Error: <" + str(e) + ">")
        return HttpResponse('', status=201)
    return HttpResponse('', status=200)

@login_required
def get_hooks(request):
    hook_l = get_hooks_helper()
    context = {'hook_l': hook_l}
    return render(request, 'core/hooks.html', context)

@login_required
def get_filecrawler(request):
    filecrawler_l = get_filecrawler_helper()
    context = {'filecrawler_l': filecrawler_l}
    return render(request, 'core/filecrawler.html', context)

@login_required
def get_systeminfo(request):
    systeminfo_l = get_systeminfo_helper()
    context = {'systeminfo_l': systeminfo_l}
    return render(request, 'core/systeminfo.html', context)

@login_required
def get_checksandbox(request):
    checksandbox_l = get_checksandbox_helper()
    context = {'checksandbox_l': checksandbox_l}
    return render(request, 'core/checksandbox.html', context)

@login_required
def get_sharpedrchecker(request):
    sharpedrchecker_l = get_sharpedrchecker_helper()
    context = {'sharpedrchecker_l': sharpedrchecker_l}
    return render(request, 'core/sharpedrchecker.html', context)

@login_required
def get_pafish(request):
    pafish_l = get_pafish_helper()
    context = {'pafish_l': pafish_l}
    return render(request, 'core/pafish.html', context)

@login_required
def get_alkhaser(request):
    alkhaser_l = get_alkhaser_helper()
    context = {'alkhaser_l': alkhaser_l}
    return render(request, 'core/alkhaser.html', context)

@login_required
def export_json(request):
    if request.method == 'GET':
        file_path = export_json_helper()
        return_file = os.path.join(settings.TEMP_FOLDER, file_path)
        response = FileResponse(open(return_file, 'rb'))
        response['content_type'] = 'application/json'
        response['Content-Disposition'] = f'attachment; filename={os.path.basename(file_path)}'
        os.remove(file_path)
        return response
    else:
        return HttpResponseRedirect('/')