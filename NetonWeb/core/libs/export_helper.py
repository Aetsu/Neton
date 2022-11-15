from django.conf import settings

from ..models import Execution, Checksandbox, Filecrawler, Hook, SharpEdrChecker, Systeminfo, OtherTools

import logging
import json
from datetime import datetime
import os

log_info = logging.getLogger('core.info')
log_error = logging.getLogger('core.error')

# https://code-maven.com/serialize-datetime-object-as-json-in-python
def datetime_converter(o):
    if isinstance(o, datetime):
        return o.__str__()

def export_json_helper():
    l_res = []
    l_execution = Execution.objects.all()
    execution_fields = [f.get_attname() for f in Execution._meta.fields]
    checksandbox_fields = [f.get_attname() for f in Checksandbox._meta.fields]
    filecrawler_fields = [f.get_attname() for f in Filecrawler._meta.fields]
    hook_fields = [f.get_attname() for f in Hook._meta.fields]
    sharpedrchecker_fields = [f.get_attname() for f in SharpEdrChecker._meta.fields]
    systeminfo_fields = [f.get_attname() for f in Systeminfo._meta.fields]
    otherTools_fields = [f.get_attname() for f in OtherTools._meta.fields]
    for e in l_execution:
        if e.wave!=0:
            d_execution = {}
            d_checksandbox = {}
            d_filecrawler = {}
            d_hook = {}
            d_sharpedrchecker = {}
            d_systeminfo = {}
            d_otherTools = {}
            execution_obj = Execution.objects.filter(executionId=e).values().first()
            for field in execution_fields:
                if execution_obj is not None:
                    d_execution[field] = execution_obj[field]
            checksandbox_obj = Checksandbox.objects.filter(execution_id=e.id).values().first()
            for field in checksandbox_fields:
                if checksandbox_obj is not None:
                    d_checksandbox[field] = checksandbox_obj[field]
            filecrawler_obj = Filecrawler.objects.filter(execution_id=e.id).values().first()
            for field in filecrawler_fields:
                if filecrawler_obj is not None:
                    d_filecrawler[field] = filecrawler_obj[field]
            hook_obj = Hook.objects.filter(execution_id=e.id).values().first()
            for field in hook_fields:
                if hook_obj is not None:
                    d_hook[field] = hook_obj[field]
            sharpedrchecker_obj = SharpEdrChecker.objects.filter(execution_id=e.id).values().first()
            for field in sharpedrchecker_fields:
                if sharpedrchecker_obj is not None:
                    d_sharpedrchecker[field] = sharpedrchecker_obj[field]
            systeminfo_obj = Systeminfo.objects.filter(execution_id=e.id).values().first()
            for field in systeminfo_fields:
                if systeminfo_obj is not None:
                    d_systeminfo[field] = systeminfo_obj[field]
            otherTools_obj = OtherTools.objects.filter(execution_id=e.id).values().first()
            for field in otherTools_fields:
                if otherTools_obj is not None:
                    d_otherTools[field] = otherTools_obj[field]
            
            d_aux = {
                'execution': d_execution,
                'checksandbox': d_checksandbox,
                'filecrawler': d_filecrawler,
                'hook': d_hook,
                'sharpedrchecker': d_sharpedrchecker,
                'systeminfo': d_systeminfo,
                'otherTools': d_otherTools,
            }
            l_res.append(d_aux)
    now = datetime.now()
    output_file = 'export_' + now.strftime("%m_%d_%Y-%H:%M:%S") + '.json'
    output_file_path = os.path.join(
        settings.TEMP_FOLDER, output_file)
    with open(output_file_path, "w") as wf: 
        json.dump(l_res, wf, default=datetime_converter)
    return output_file_path

