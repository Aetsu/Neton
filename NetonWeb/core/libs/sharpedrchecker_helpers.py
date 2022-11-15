import core.libs.helpers as helpers
import core.libs.execution_helpers as execution_helpers
from ..models import SharpEdrChecker
import logging

log_info = logging.getLogger('core.info')
log_error = logging.getLogger('core.error')

def insert(json_obj, execution_obj, executionId):
    try:
        exists = SharpEdrChecker.objects.filter(execution=execution_obj.id).first()
        if not exists:
            h_obj = SharpEdrChecker(
                execution = execution_obj,
                launchProcesses = helpers.b64decode(json_obj['launchProcesses']),
                currentProcessModules = helpers.b64decode(json_obj['currentProcessModules']),
                launchCheckDirectories = helpers.b64decode(json_obj['launchCheckDirectories']),
                launchServiceChecker = helpers.b64decode(json_obj['launchServiceChecker']),
                launchCheckDrivers = helpers.b64decode(json_obj['launchCheckDrivers']),
            )
            h_obj.save()
            execution_helpers.update_execution_module(executionId, 'sharpedrchecker')
            log_info.info('insert_sharpedrchecker <executionId: ' + executionId + '>')
    except Exception as e:
        log_error.error('insert_sharpedrchecker <executionId: ' + executionId + '> - ' + str(e))

def get_sharpedrchecker_helper(execution_id=None):
    sharp_l = []
    try:
        if execution_id == None:
            sharp_obj = SharpEdrChecker.objects.values()
        else:
            sharp_obj = SharpEdrChecker.objects.filter(execution_id=execution_id).values()
        for elem in sharp_obj:
            execution_info = execution_helpers.get_execution_id(elem['execution_id'])
            launchProcesses = elem['launchProcesses'].split('\n')
            launchProcesses = helpers.remove_empty_elements(launchProcesses)
            launchProcesses_aux = []
            for s in launchProcesses:
                s_t = s.split(" : ")
                if len(s_t) > 1:
                    launchProcesses_aux.append({
                        'name': s_t[0].strip(),
                        'tag': s_t[1].strip(),
                    })
            currentProcessModules = elem['currentProcessModules'].split('\n')
            currentProcessModules = helpers.remove_empty_elements(currentProcessModules)
            currentProcessModules_aux = []
            for s in currentProcessModules:
                s_t = s.split(" : ")
                if len(s_t) > 1:
                    currentProcessModules_aux.append({
                        'name': s_t[0].strip(),
                        'tag': s_t[1].strip(),
                    })
            launchCheckDirectories = elem['launchCheckDirectories'].split('\n')
            launchCheckDirectories = helpers.remove_empty_elements(launchCheckDirectories)
            launchCheckDirectories_aux = []
            for s in launchCheckDirectories:
                s_t = s.split(" : ")
                if len(s_t) > 1:
                    launchCheckDirectories_aux.append({
                        'name': s_t[0].strip(),
                        'tag': s_t[1].strip(),
                    })
            launchServiceChecker = elem['launchServiceChecker'].split('\n')
            launchServiceChecker = helpers.remove_empty_elements(launchServiceChecker)
            launchServiceChecker_aux = []
            for s in launchServiceChecker:
                s_t = s.split(" : ")
                if len(s_t) > 1:
                    launchServiceChecker_aux.append({
                        'name': s_t[0].strip(),
                        'tag': s_t[1].strip(),
                    })
            launchCheckDrivers = elem['launchCheckDrivers'].split('\n')
            launchCheckDrivers = helpers.remove_empty_elements(launchCheckDrivers)
            launchCheckDrivers_aux = []
            for s in launchCheckDrivers:
                s_t = s.split(" : ")
                if len(s_t) == 2:
                    launchCheckDrivers_aux.append({
                        'name': s_t[0].strip(),
                        'tag': s_t[1].strip(),
                    })
            sharp_l.append({
                'executionId': execution_info.executionId,
                'ip': execution_info.ip,
                'ip_country_code': execution_info.ip_country_code,
                'ip_asn_description': execution_info.ip_asn_description,
                'sandboxId': execution_info.sandboxId,
                'wave': execution_info.wave,
                'launchProcesses': launchProcesses_aux,
                'currentProcessModules': currentProcessModules_aux,
                'launchCheckDirectories': launchCheckDirectories_aux,
                'launchServiceChecker': launchServiceChecker_aux,
                'launchCheckDrivers': launchCheckDrivers_aux,
                'creation_date': elem['creation_date'].strftime("%Y-%m-%d %H:%M:%S"),
            })
    except Exception as e:
        log_error.error('get_sharpedrchecker_helper - ' + str(e))
    return sharp_l