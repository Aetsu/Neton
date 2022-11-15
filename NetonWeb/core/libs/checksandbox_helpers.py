import core.libs.helpers as helpers
import core.libs.execution_helpers as execution_helpers
from ..models import Checksandbox
import logging

log_info = logging.getLogger('core.info')
log_error = logging.getLogger('core.error')

def insert(json_obj, execution_obj, executionId):
    try:
        exists = Checksandbox.objects.filter(execution=execution_obj.id).first()
        if not exists:
            h_obj = Checksandbox(
                execution = execution_obj,
                checkFilesTags = helpers.b64decode(json_obj['checkFilesTags']),
                checkExeRootTags = helpers.b64decode(json_obj['checkExeRootTags']),
                checkPathTags = helpers.b64decode(json_obj['checkPathTags']),
                checkKeyValueTags = helpers.b64decode(json_obj['checkKeyValueTags']),
                checkWindowTitleTags = helpers.b64decode(json_obj['checkWindowTitleTags']),
                checkNWindowsTags = helpers.b64decode(json_obj['checkNWindowsTags']),
                checkDebugPrivsTags = helpers.b64decode(json_obj['checkDebugPrivsTags']),
                checkHdName = helpers.b64decode(json_obj['checkHdName']),
                checkTemp = helpers.b64decode(json_obj['checkTemp']),
            )
            h_obj.save()
            execution_helpers.update_execution_module(executionId, 'checksandbox')
            log_info.info('insert_checksandbox <executionId: ' + executionId + '>')
    except Exception as e:
        log_error.error('insert_checksandbox <executionId: ' + executionId + '> - ' + str(e))

def get_checksandbox_helper(execution_id=None):
    checksandbox_l = []
    try:
        if execution_id == None:
            checksandbox_obj = Checksandbox.objects.values()
        else:
            checksandbox_obj = Checksandbox.objects.filter(execution_id=execution_id).values()

        for elem in checksandbox_obj:
            execution_info = execution_helpers.get_execution_id(elem['execution_id'])
            checkFilesTags_l = helpers.remove_empty_lists(elem['checkFilesTags'].split('\n'))
            checkFilesTags_l_aux = []
            for s in checkFilesTags_l:
                s_t = s.split("|")
                checkFilesTags_l_aux.append({
                    'tag': s_t[0].strip(),
                    'value': s_t[1].strip(),
                })
            checkExeRootTags_l = helpers.remove_empty_lists(elem['checkExeRootTags'].split('\n'))
            checkExeRootTags_l_aux = []
            for s in checkExeRootTags_l:
                s_t = s.split("|")
                checkExeRootTags_l_aux.append({
                    'tag': s_t[0].strip(),
                    'value': s_t[1].strip(),
                })
            checkPathTags_l = helpers.remove_empty_lists(elem['checkPathTags'].split('\n'))
            checkPathTags_l_aux = []
            for s in checkPathTags_l:
                s_t = s.split("|")
                checkPathTags_l_aux.append({
                    'tag': s_t[0].strip(),
                    'value': s_t[1].strip(),
                })
            checkKeyValueTags_l = helpers.remove_empty_lists(elem['checkKeyValueTags'].split('\n'))
            checkKeyValueTags_l_aux = []
            for s in checkKeyValueTags_l:
                s_t = s.split("|")
                checkKeyValueTags_l_aux.append({
                    'tag': s_t[0].strip(),
                    'value': s_t[1].strip(),
                })
            checkWindowTitleTags_l = helpers.remove_empty_lists(elem['checkWindowTitleTags'].split('\n'))
            checkWindowTitleTags_l_aux = []
            for s in checkWindowTitleTags_l:
                s_t = s.split("|")
                checkWindowTitleTags_l_aux.append({
                    'tag': s_t[0].strip(),
                    'value': s_t[1].strip(),
                })
            checkNWindowsTags_aux = elem['checkNWindowsTags'].split(' | ')
            checkDebugPrivsTags_aux = elem['checkDebugPrivsTags'].split(' | ')
            checkHdName_l = helpers.remove_empty_lists(elem['checkHdName'].split('\n'))
            checkHdName_l_aux = []
            for s in checkHdName_l:
                s_t = s.split("|")
                checkHdName_l_aux.append({
                    'tag': s_t[0].strip(),
                    'value': s_t[1].strip(),
                })
            checkTemp_aux = elem['checkTemp'].split(' | ')
            checksandbox_l.append({
                'executionId': execution_info.executionId,
                'ip': execution_info.ip,
                'ip_country_code': execution_info.ip_country_code,
                'ip_asn_description': execution_info.ip_asn_description,
                'sandboxId': execution_info.sandboxId,
                'wave': execution_info.wave,
                'checkFilesTags': checkFilesTags_l_aux,
                'checkExeRootTags': checkExeRootTags_l_aux,
                'checkPathTags': checkPathTags_l_aux,
                'checkKeyValueTags': checkKeyValueTags_l_aux,
                'checkWindowTitleTags': checkWindowTitleTags_l_aux,
                'checkNWindowsTags': checkNWindowsTags_aux[1],
                'checkDebugPrivsTags': checkDebugPrivsTags_aux[1],
                'checkHdName': checkHdName_l_aux,
                'checkTemp': checkTemp_aux[1],
                'creation_date': elem['creation_date'],
            })
    except Exception as e:
        log_error.error('get_checksandbox_helper - ' + str(e))
    return checksandbox_l