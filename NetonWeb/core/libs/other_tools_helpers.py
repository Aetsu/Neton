import core.libs.helpers as helpers
import core.libs.execution_helpers as execution_helpers
from ..models import OtherTools
import logging

log_info = logging.getLogger('core.info')
log_error = logging.getLogger('core.error')

def insertPafish(json_obj, execution_obj, executionId):
    try:
        exists = OtherTools.objects.filter(execution=execution_obj.id).first()
        if not exists:
            h_obj = OtherTools(
                execution = execution_obj,
                pafish = helpers.b64decode(json_obj['moduleOutput']),
            )
            h_obj.save()
            execution_helpers.update_execution_module(executionId, 'pafish')
            log_info.info('insertPafish <executionId: ' + executionId + '>')
        else:
            exists.pafish = helpers.b64decode(json_obj['moduleOutput'])
            exists.save()
            execution_helpers.update_execution_module(executionId, 'pafish')
    except Exception as e:
        log_error.error('insertPafish <executionId: ' + executionId + '> - ' + str(e))
    pass

def insertAlkhaser(json_obj, execution_obj, executionId):
    try:
        exists = OtherTools.objects.filter(execution=execution_obj.id).first()
        if not exists:
            h_obj = OtherTools(
                execution = execution_obj,
                alkhaser = helpers.b64decode(json_obj['moduleOutput']),
            )
            h_obj.save()
            execution_helpers.update_execution_module(executionId, 'alkhaser')
            log_info.info('insertAlkhaser <executionId: ' + executionId + '>')
        else:
            exists.alkhaser = helpers.b64decode(json_obj['moduleOutput'])
            exists.save()
            execution_helpers.update_execution_module(executionId, 'alkhaser')
    except Exception as e:
        log_error.error('insertAlkhaser <executionId: ' + executionId + '> - ' + str(e))


def get_pafish_helper(execution_id=None):
    pafish_l = []
    try:
        if execution_id == None:
            pafish_obj = OtherTools.objects.values()
        else:
            pafish_obj = OtherTools.objects.filter(execution_id=execution_id).values()
        for elem in pafish_obj:
            execution_info = execution_helpers.get_execution_id(elem['execution_id'])
            pafish_l.append({
                'executionId': execution_info.executionId,
                'ip': execution_info.ip,
                'ip_country_code': execution_info.ip_country_code,
                'ip_asn_description': execution_info.ip_asn_description,
                'sandboxId': execution_info.sandboxId,
                'wave': execution_info.wave,
                'pafish': elem['pafish'].split('\n'),
                'creation_date': elem['creation_date'].strftime("%Y-%m-%d %H:%M:%S"),
            })
    except Exception as e:
        log_error.error('get_pafish_helper - ' + str(e))
    return pafish_l

def get_alkhaser_helper(execution_id=None):
    alkhaser_l = []
    try:
        if execution_id == None:
            alkhaser_obj = OtherTools.objects.values()
        else:
            alkhaser_obj = OtherTools.objects.filter(execution_id=execution_id).values()
        for elem in alkhaser_obj:
            execution_info = execution_helpers.get_execution_id(elem['execution_id'])
            alkhaser_l.append({
                'executionId': execution_info.executionId,
                'ip': execution_info.ip,
                'ip_country_code': execution_info.ip_country_code,
                'ip_asn_description': execution_info.ip_asn_description,
                'sandboxId': execution_info.sandboxId,
                'wave': execution_info.wave,
                'alkhaser': elem['alkhaser'].split('\n'),
                'creation_date': elem['creation_date'].strftime("%Y-%m-%d %H:%M:%S"),
            })
    except Exception as e:
        log_error.error('get_alkhaser_helper - ' + str(e))
    return alkhaser_l