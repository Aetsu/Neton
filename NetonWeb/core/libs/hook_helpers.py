import core.libs.helpers as helpers
import core.libs.execution_helpers as execution_helpers
from ..models import Hook
import logging

log_info = logging.getLogger('core.info')
log_error = logging.getLogger('core.error')

def insert(json_obj, execution_obj, executionId):
    try:
        exists = Hook.objects.filter(execution=execution_obj.id).first()
        if not exists:
            h_obj = Hook(
                execution = execution_obj,
                hooklist = helpers.b64decode(json_obj['hooklist'])
            )
            h_obj.save()
            execution_helpers.update_execution_module(executionId, 'hook')
            log_info.info('insert_hooks <executionId: ' + executionId + '>')
    except Exception as e:
        log_error.error('insert_hooks <executionId: ' + executionId + '> - ' + str(e))

def get_hooks_helper(execution_id=None):
    hook_l = []
    try:
        if execution_id == None:
            hook_obj = Hook.objects.values()
        else:
            hook_obj = Hook.objects.filter(execution_id=execution_id).values()
        for elem in hook_obj:
            execution_info = execution_helpers.get_execution_id(elem['execution_id'])
            
            s_hooks = elem['hooklist'].split('\n')
            # hook_l_aux = [line for line in s_hooks if line.find('DETECTED')!= -1]
            hook_l_aux = [line.split(' - ') for line in s_hooks]
            # print(hook_l_aux)
            hook_l_aux = [{'hook': line[0].strip(), 'status': line[1].strip()} for line in hook_l_aux[1:-1] if len(line)>1]
            hook_l.append({
                'executionId': execution_info.executionId,
                'ip': execution_info.ip,
                'ip_country_code': execution_info.ip_country_code,
                'ip_asn_description': execution_info.ip_asn_description,
                'sandboxId': execution_info.sandboxId,
                'wave': execution_info.wave,
                'hooklist': hook_l_aux,
                'creation_date': elem['creation_date'].strftime("%Y-%m-%d %H:%M:%S"),
            })
    except Exception as e:
        log_error.error('get_hooks_helper - ' + str(e))
    return hook_l