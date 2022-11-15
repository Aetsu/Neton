import core.libs.helpers as helpers
import core.libs.execution_helpers as execution_helpers
from ..models import Systeminfo
import logging

log_info = logging.getLogger('core.info')
log_error = logging.getLogger('core.error')

def insert(json_obj, execution_obj, executionId):
    try:
        exists = Systeminfo.objects.filter(execution=execution_obj.id).first()
        if not exists:
            l_fields = ['modules', 'processorInformation', 'physicalMemory', 'biosSerNo', 'networkInfo', 'tasklist', 'users', 'localgroup', 'netstat', 'services', 'dirDrivers', 'lDrives', 'computerRAM', 'environmentVariables', 'systemPipes', 'screenshot', 
            'osVersion', 'screenRes', 'nScreens', 'hDSize', 'systemUptime', 'recentFiles', 'installedApps', 'mousePosition', 'sysinfoOutput']
            for f in l_fields:
                if not f in json_obj:
                    json_obj[f] = ''
            h_obj = Systeminfo(
                execution = execution_obj,
                modules = helpers.b64decode(json_obj['modules']),
                processor_information = helpers.b64decode(json_obj['processorInformation']),
                physical_memory = helpers.b64decode(json_obj['physicalMemory']),
                bios_ser_no = helpers.b64decode(json_obj['biosSerNo']),
                networkInfo = helpers.b64decode(json_obj['networkInfo']),
                tasklist = helpers.b64decode(json_obj['tasklist']),
                users = helpers.b64decode(json_obj['users']),
                localgroup = helpers.b64decode(json_obj['localgroup']),
                netstat = helpers.b64decode(json_obj['netstat']),
                services = helpers.b64decode(json_obj['services']),
                dir_drivers = helpers.b64decode(json_obj['dirDrivers']),
                l_drives = helpers.b64decode(json_obj['lDrives']),
                computer_ram = helpers.b64decode(json_obj['computerRAM']),
                environment_variables = helpers.b64decode(json_obj['environmentVariables']),
                system_pipes = helpers.b64decode(json_obj['systemPipes']),
                screenshot = json_obj['screenshot'],
                os_version = helpers.b64decode(json_obj['osVersion']),
                screen_res = helpers.b64decode(json_obj['screenRes']),
                n_screens = helpers.b64decode(json_obj['nScreens']),
                hd_size = helpers.b64decode(json_obj['hDSize']),
                system_uptime = helpers.b64decode(json_obj['systemUptime']),
                recent_files = helpers.b64decode(json_obj['recentFiles']),
                installed_apps = helpers.b64decode(json_obj['installedApps']),
                mouse_position = helpers.b64decode(json_obj['mousePosition']),
                sysinfo_output = helpers.b64decode(json_obj['sysinfoOutput']),
            )
            h_obj.save()
            execution_helpers.update_execution_module(executionId, 'systeminfo')
            log_info.info('insert_systeminfo <executionId: ' + executionId + '>')
        else:
            # for k, v in json_obj.items():
            if 'processorInformation' in json_obj:
                exists.processor_information = helpers.b64decode(json_obj['processorInformation'])
            if 'physicalMemory' in json_obj:
                exists.physical_memory = helpers.b64decode(json_obj['physicalMemory'])
            if 'biosSerNo' in json_obj:
                exists.bios_ser_no = helpers.b64decode(json_obj['biosSerNo'])
            if 'networkInfo' in json_obj:
                exists.networkInfo = helpers.b64decode(json_obj['networkInfo'])
            if 'tasklist' in json_obj:
                exists.tasklist = helpers.b64decode(json_obj['tasklist'])
            if 'users' in json_obj:
                exists.users = helpers.b64decode(json_obj['users'])
            if 'localgroup' in json_obj:
                exists.localgroup = helpers.b64decode(json_obj['localgroup'])
            if 'netstat' in json_obj:
                exists.netstat = helpers.b64decode(json_obj['netstat'])
            if 'services' in json_obj:
                exists.services = helpers.b64decode(json_obj['services'])
            if 'dirDrivers' in json_obj:
                exists.dir_drivers = helpers.b64decode(json_obj['dirDrivers'])
            if 'lDrives' in json_obj:
                exists.l_drives = helpers.b64decode(json_obj['lDrives'])
            if 'computerRAM' in json_obj:
                exists.computer_ram = helpers.b64decode(json_obj['computerRAM'])
            if 'environmentVariables' in json_obj:
                exists.environment_variables = helpers.b64decode(json_obj['environmentVariables'])
            if 'systemPipes' in json_obj:
                exists.system_pipes = helpers.b64decode(json_obj['systemPipes'])
            if 'screenshot' in json_obj:
                exists.screenshot = json_obj['screenshot']
            if 'osVersion' in json_obj:
                exists.os_version = helpers.b64decode(json_obj['osVersion'])
            if 'screenRes' in json_obj:
                exists.screen_res = helpers.b64decode(json_obj['screenRes'])
            if 'nScreens' in json_obj:
                exists.n_screens = helpers.b64decode(json_obj['nScreens'])
            if 'hDSize' in json_obj:
                exists.hd_size = helpers.b64decode(json_obj['hDSize'])
            if 'systemUptime' in json_obj:
                exists.system_uptime = helpers.b64decode(json_obj['systemUptime'])
            if 'recentFiles' in json_obj:
                exists.recent_files = helpers.b64decode(json_obj['recentFiles'])
            if 'installedApps' in json_obj:
                exists.installed_apps = helpers.b64decode(json_obj['installedApps'])
            if 'mousePosition' in json_obj:
                exists.mouse_position = helpers.b64decode(json_obj['mousePosition'])
            if 'sysinfoOutput' in json_obj:
                exists.sysinfo_output = helpers.b64decode(json_obj['sysinfoOutput'])
            exists.save()
    except Exception as e:
        log_error.error('insert_systeminfo <executionId: ' + executionId + '> - ' + str(e))

def get_systeminfo_helper(execution_id=None):
    systeminfo_l = []
    try:
        if execution_id == None:
            systeminfo_obj = Systeminfo.objects.values()
        else:
            systeminfo_obj = Systeminfo.objects.filter(execution_id=execution_id).values()
        for elem in systeminfo_obj:
            execution_info = execution_helpers.get_execution_id(elem['execution_id'])
            modules_l = elem['modules'].split('\n')
            modules_l_aux = []
            for m in modules_l:
                s_m = m.split("|")
                try:
                    modules_l_aux.append({
                        'name': s_m[0].strip(),
                        'path': s_m[1].strip(),
                        'description': s_m[2].strip(),
                    })
                except:
                    pass
            tasklist_l = elem['tasklist'].split('\n')
            tasklist_l = helpers.remove_empty_elements(tasklist_l)
            task_l_aux = []
            for t in tasklist_l:
                s_t = t.split("|")
                try:
                    task_l_aux.append({
                        'name': s_t[0].strip(),
                        'pid': s_t[1].strip(),
                        'title': s_t[2].strip(),
                        'session': s_t[3].strip(),
                        'user': s_t[4].strip(),
                    })
                except:
                    pass
            service_l = elem['services'].split('\n')
            service_l = helpers.remove_empty_elements(service_l)
            services_l_aux = []
            for s in service_l:
                s_t = s.split("|")
                try:
                    services_l_aux.append({
                        'name': s_t[0].strip(),
                        'desc': s_t[1].strip(),
                        'status': s_t[2].strip(),
                    })
                except:
                    pass
            netstat_l = elem['netstat'].split('\n')
            netstat_l = helpers.remove_empty_elements(netstat_l)
            netstats_l_aux = []
            for s in netstat_l:
                s_t = s.split("|")
                try:
                    netstats_l_aux.append({
                        'src': s_t[0].strip(),
                        'dest': s_t[1].strip(),
                        'status': s_t[2].strip(),
                    })
                except:
                    pass
            environment_variables_l = elem['environment_variables'].split('\n')
            environment_variables_l = helpers.remove_empty_elements(environment_variables_l)
            environment_variables_l_aux = []
            for s in environment_variables_l:
                s_t = s.split("|")
                try:
                    environment_variables_l_aux.append({
                        'variable': s_t[0].strip(),
                        'value': s_t[1].strip(),
                    })
                except:
                    pass
            os_version = elem['os_version'].split('|')
            if len(os_version) > 2:
                os_version_aux = os_version[1].strip()
            else:
                os_version_aux = elem['os_version']
            
            installed_apps_l = elem['installed_apps'].split('\n')
            installed_apps_l = helpers.remove_empty_elements(installed_apps_l)
            installed_apps_l_aux = []
            for s in installed_apps_l:
                s_t = s.split("|")
                app = ''
                path = ''
                if len(s_t) > 1:
                    app = s_t[0].strip()
                    path = s_t[1].strip()
                else:
                    app = s_t[0].strip()
                installed_apps_l_aux.append({
                    'app': app,
                    'path': path,
                    })
            mouse_position_l = elem['mouse_position'].split('\n')
            mouse_position_l = helpers.remove_empty_elements(mouse_position_l)
            mouse_position_l_aux = []
            for s in mouse_position_l:
                s_t = s.split("|")
                try:
                    mouse_position_l_aux.append({
                        'x': s_t[0].strip(),
                        'y': s_t[1].strip(),
                    })
                except:
                    pass
            networkInfo_l = elem['networkInfo'].split('\n')
            networkInfo_l = helpers.remove_empty_elements(networkInfo_l)
            networkInfo_l_aux = []
            for s in networkInfo_l:
                s_t = s.split("|")
                try:
                    if not 'loopback' in s_t[0].lower():
                        networkInfo_l_aux.append({
                            'name': s_t[0].strip(),
                            'mac': s_t[1].strip(),
                        })
                except:
                    pass
            dir_drivers_l = helpers.remove_empty_elements(elem['dir_drivers'].split('*********'))
            dir_drivers_l_aux = []
            for aux in dir_drivers_l:
                dir_drivers_l_aux.append(
                    helpers.remove_empty_elements(aux.split('\n')))
            systeminfo_l.append({
                'executionId': execution_info.executionId,
                'ip': execution_info.ip,
                'ip_country_code': execution_info.ip_country_code,
                'ip_asn_description': execution_info.ip_asn_description,
                'sandboxId': execution_info.sandboxId,
                'wave': execution_info.wave,
                'sysinfo_output': helpers.remove_empty_elements(elem['sysinfo_output'].split('\n')),
                'os_version' : os_version_aux,
                'processor_information' : helpers.remove_empty_elements(elem['processor_information'].split('\n')),
                'physical_memory' : helpers.remove_empty_elements(elem['physical_memory'].split('\n')),
                'computer_ram' : helpers.remove_empty_elements(elem['computer_ram'].split('\n')),
                'bios_ser_no' : helpers.remove_empty_elements(elem['bios_ser_no'].split('\n')),
                'networkInfo' : networkInfo_l_aux,
                'tasklist' : task_l_aux,
                'users' : helpers.remove_empty_elements(elem['users'].split('\n')),
                'localgroup' : helpers.remove_empty_elements(elem['localgroup'].split('\n')),
                'netstat' : netstats_l_aux,
                'services' : services_l_aux,
                'dir_drivers' : helpers.remove_empty_elements(dir_drivers_l_aux),
                'l_drives' : elem['l_drives'].split('\n'),
                'environment_variables' : environment_variables_l_aux,
                'system_pipes' : helpers.remove_empty_elements(elem['system_pipes'].split('\n')),
                'screen_res' : elem['screen_res'].replace(' | ', 'x').strip(),
                'n_screens' : elem['n_screens'].strip(),
                'hd_size': elem['hd_size'].replace(' | ', ' ').strip(),
                'system_uptime': elem['system_uptime'].strip(),
                'screenshot': elem['screenshot'].strip(),
                'recent_files': helpers.remove_empty_elements(elem['recent_files'].split('\n')),
                'installed_apps': installed_apps_l_aux,
                'mouse_position':mouse_position_l_aux,
                'modules': modules_l_aux,
                'creation_date': elem['creation_date'].strftime("%Y-%m-%d %H:%M:%S"),
            })
            
    except Exception as e:
        log_error.error('get_systeminfo_helper - ' + str(e))
    return systeminfo_l