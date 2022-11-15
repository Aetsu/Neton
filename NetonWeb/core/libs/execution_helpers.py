from genericpath import exists
import os
from django.conf import settings
import time

import core.libs.helpers as helpers
import core.libs.hook_helpers as hook_helpers
import core.libs.systeminfo_helpers as systeminfo_helpers
import core.libs.filecrawler_helpers as filecrawler_helpers
import core.libs.checksandbox_helpers as checksandbox_helpers
import core.libs.sharpedrchecker_helpers as sharpedrchecker_helpers
import core.libs.other_tools_helpers as other_tools_helpers
import core.libs.ip_helpers as ip_helpers
from ..models import Execution, Targets

import logging

log_info = logging.getLogger('core.info')
log_error = logging.getLogger('core.error')

def check_module(json_obj, ip):
    executionId = 'error'
    try:
        executionId = helpers.b64decode(json_obj['executionId'])
        sandboxId = helpers.b64decode(json_obj['sandboxId'])
        wave = helpers.b64decode(json_obj['wave'])
        version = helpers.b64decode(json_obj['version'])
        md5_sample = helpers.b64decode(json_obj['md5Hash'])
        sha256_sample = helpers.b64decode(json_obj['sha256Hash'])
        execution_obj = get_execution_helper(executionId)
        if execution_obj is None:
            execution_obj = insert_execution(executionId, sandboxId, wave, ip, version, md5_sample, sha256_sample)
        if 'module' in json_obj:
            module = helpers.b64decode(json_obj['module'])
            if module == 'checkhooks':
                hook_helpers.insert(json_obj, execution_obj, executionId)
            elif module == 'systeminfo':
                systeminfo_helpers.insert(json_obj, execution_obj, executionId)
            elif module == 'filecrawler':
                filecrawler_helpers.insert(json_obj, execution_obj, executionId)
            elif module == 'checksandbox':
                checksandbox_helpers.insert(json_obj, execution_obj, executionId)
            elif module == 'sharpedrchecker':
                sharpedrchecker_helpers.insert(json_obj, execution_obj, executionId)
            elif module == 'pafish':
                other_tools_helpers.insertPafish(json_obj, execution_obj, executionId)
            elif module == 'alkhaser':
                other_tools_helpers.insertAlkhaser(json_obj, execution_obj, executionId)
    except Exception as e:
        log_error.error('check_module <executionId: ' + executionId + '> - ' + str(e))




def insert_execution(executionId, sandboxId, wave, ip, version, md5_sample, sha256_sample):
    ip_country_code = ''
    ip_asn_description = ''
    try:
        ip_info = ip_helpers.analyze_ip(ip)
        if 'asn_country_code' in ip_info:
            ip_country_code = ip_info['asn_country_code']
        if 'asn_description' in ip_info:
            ip_asn_description = ip_info['asn_description']
    except Exception as e:
        log_error.error('insert_execution - analyze_ip <IP: ' + str(ip) + '> - ' + str(e))

    try:
        exists = Execution.objects.filter(executionId=executionId).first()
        if not exists:
            s_obj = Execution(
                executionId = executionId,
                sandboxId = sandboxId,
                wave = wave,
                ip = ip,
                ip_country_code = ip_country_code,
                ip_asn_description = ip_asn_description,
                version = version,
                md5_sample = md5_sample,
                sha256_sample = sha256_sample
            )
            s_obj.save()
            log_info.info('insert_execution <executionId: ' + executionId + '>')
            return s_obj
        else:
            return exists
    except Exception as e:
        log_error.error('insert_execution <executionId: ' + executionId + '> - ' + str(e))
        return None

def get_execution_helper(executionId):
    execution_q = None
    try:
        execution_q = Execution.objects.filter(executionId=executionId).first()
    except Exception as e:
        log_error.error('get_execution <executionId: ' + executionId + '> - ' + str(e))
    return execution_q

def get_execution_id(id):
    execution_q = None
    try:
        execution_q = Execution.objects.filter(id=id).first()
    except Exception as e:
        log_error.error('get_execution_id <id: ' + id + '> - ' + str(e))
    return execution_q

def update_execution_module(executionId, module_name):
    execution_obj = get_execution_helper(executionId)
    if execution_obj is not None:
        if module_name == 'hook':
            execution_obj.m_hook = 1
        elif module_name == 'systeminfo':
            execution_obj.m_systeminfo = 1
        elif module_name == 'filecrawler':
            execution_obj.m_filecrawler = 1
        elif module_name == 'checksandbox':
            execution_obj.m_checksandbox = 1
        elif module_name == 'sharpedrchecker':
            execution_obj.m_sharpedrchecker = 1
        elif module_name == 'pafish':
            execution_obj.m_pafish = 1
        elif module_name == 'alkhaser':
            execution_obj.m_alkhaser = 1
        execution_obj.save()


def get_executions_helper():
    execution_q = None
    l_res = []
    try:
        execution_q = Execution.objects.values()
        for elem in  execution_q:
            if (elem['m_hook'] == 0 and elem['m_systeminfo'] == 0 \
                and elem['m_filecrawler'] == 0 and elem['m_checksandbox'] == 0 \
                    and elem['m_sharpedrchecker'] == 0 and elem['m_pafish'] == 0 \
                        and elem['m_alkhaser'] == 0):
                        pass
            else:
                elem['creation_date'] = elem['creation_date'].strftime("%Y-%m-%d %H:%M:%S")
                l_res.append(elem)
    except Exception as e:
        log_error.error('get_executions - ' + str(e))
    return l_res