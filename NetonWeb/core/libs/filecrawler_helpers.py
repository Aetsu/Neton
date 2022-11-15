import core.libs.helpers as helpers
import core.libs.execution_helpers as execution_helpers
from ..models import Filecrawler
import logging

log_info = logging.getLogger('core.info')
log_error = logging.getLogger('core.error')

def insert(json_obj, execution_obj, executionId):
    try:
        exists = Filecrawler.objects.filter(execution=execution_obj.id).first()
        if not exists:
            h_obj = Filecrawler(
                execution = execution_obj,
                files_in_drives = helpers.b64decode(json_obj['sFilesInDrives']),
                file_paths_64 = helpers.b64decode(json_obj['sProgramFiles64']),
                file_paths_32 = helpers.b64decode(json_obj['sProgramFiles86']),
                dir_c = helpers.b64decode(json_obj['sDirC']),
                drivers = helpers.b64decode(json_obj['sDrivers']),
            )
            h_obj.save()
            execution_helpers.update_execution_module(executionId, 'filecrawler')
            log_info.info('insert_filecrawler <executionId: ' + executionId + '>')
    except Exception as e:
        log_error.error('insert_filecrawler <executionId: ' + executionId + '> - ' + str(e))

def get_filecrawler_helper(execution_id=None):
    filecrawler_l = []
    try:
        if execution_id == None:
            filecrawler_obj = Filecrawler.objects.values()
        else:
            filecrawler_obj = Filecrawler.objects.filter(execution_id=execution_id).values()
        for elem in filecrawler_obj:
            execution_info = execution_helpers.get_execution_id(elem['execution_id'])
            filecrawler_l.append({
                'executionId': execution_info.executionId,
                'ip': execution_info.ip,
                'ip_country_code': execution_info.ip_country_code,
                'ip_asn_description': execution_info.ip_asn_description,
                'sandboxId': execution_info.sandboxId,
                'wave': execution_info.wave,
                'files_in_drives' : helpers.remove_empty_elements(elem['files_in_drives'].split('\n')),
                'file_paths_64' : helpers.remove_empty_elements(elem['file_paths_64'].split('\n')),
                'file_paths_32' : helpers.remove_empty_elements(elem['file_paths_32'].split('\n')),
                'dir_c' : helpers.remove_empty_elements(elem['dir_c'].split('\n')),
                'drivers' : helpers.remove_empty_elements(elem['drivers'].split('\n')),
                'creation_date': elem['creation_date'].strftime("%Y-%m-%d %H:%M:%S"),
            })
    except Exception as e:
        log_error.error('get_filecrawler_helper - ' + str(e))
    return filecrawler_l