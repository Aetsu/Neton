from django.db import models
from django.utils import timezone
from django.conf import settings
from datetime import datetime

class Targets(models.Model):
    name = models.CharField(max_length=100, default='')
    sandboxId = models.IntegerField(default=0)

    def __str__(self):
        return self.name

class Execution(models.Model):
    executionId = models.CharField(max_length=100)
    sandboxId = models.CharField(max_length=100, default='')
    wave = models.IntegerField(default=0)
    m_hook = models.IntegerField(default=0)
    m_systeminfo = models.IntegerField(default=0)
    m_filecrawler = models.IntegerField(default=0)
    m_checksandbox = models.IntegerField(default=0)
    m_sharpedrchecker = models.IntegerField(default=0)
    m_pafish = models.IntegerField(default=0)
    m_alkhaser = models.IntegerField(default=0)
    ip = models.GenericIPAddressField(blank=True, null=True)
    ip_country_code = models.TextField(default='')
    ip_asn_description = models.TextField(default='')
    md5_sample = models.TextField(default='')
    sha256_sample = models.TextField(default='')
    version = models.CharField(max_length=20, default='0')
    creation_date = models.DateTimeField(default=timezone.now)

    def __str__(self):
        return self.executionId

class Hook(models.Model):
    execution = models.ForeignKey(Execution, null=True, on_delete=models.CASCADE)
    hooklist = models.TextField(default='')
    creation_date = models.DateTimeField(default=timezone.now)

    def __str__(self):
        return str(self.execution)

class Systeminfo(models.Model):
    execution = models.ForeignKey(Execution, null=True, on_delete=models.CASCADE)
    modules = models.TextField(default='')
    processor_information = models.TextField(default='')
    physical_memory = models.TextField(default='')
    bios_ser_no = models.TextField(default='')
    networkInfo = models.TextField(default='')
    tasklist = models.TextField(default='')
    users = models.TextField(default='')
    localgroup = models.TextField(default='')
    netstat = models.TextField(default='')
    services = models.TextField(default='')
    dir_drivers = models.TextField(default='')
    l_drives = models.TextField(default='')
    computer_ram  = models.TextField(default='')
    environment_variables  = models.TextField(default='')
    system_pipes  = models.TextField(default='')
    screenshot  = models.TextField(default='')
    os_version  = models.TextField(default='')
    screen_res  = models.TextField(default='')
    n_screens  = models.TextField(default='')
    hd_size  = models.TextField(default='')
    system_uptime  = models.TextField(default='')
    recent_files  = models.TextField(default='')
    installed_apps  = models.TextField(default='')
    mouse_position  = models.TextField(default='')
    sysinfo_output  = models.TextField(default='')
    creation_date = models.DateTimeField(default=timezone.now)

    def __str__(self):
        return str(self.execution)

class Filecrawler(models.Model):
    execution = models.ForeignKey(Execution, null=True, on_delete=models.CASCADE)
    files_in_drives = models.TextField(default='')
    file_paths_64 = models.TextField(default='')
    file_paths_32 = models.TextField(default='')
    dir_c = models.TextField(default='')
    drivers = models.TextField(default='')
    creation_date = models.DateTimeField(default=timezone.now)

    def __str__(self):
        return str(self.execution)

class SharpEdrChecker(models.Model):
    execution = models.ForeignKey(Execution, null=True, on_delete=models.CASCADE)
    launchProcesses = models.TextField(default='')
    currentProcessModules = models.TextField(default='')
    launchCheckDirectories = models.TextField(default='')
    launchServiceChecker = models.TextField(default='')
    launchCheckDrivers = models.TextField(default='')
    creation_date = models.DateTimeField(default=timezone.now)

    def __str__(self):
        return str(self.execution)

class Checksandbox(models.Model):
    execution = models.ForeignKey(Execution, null=True, on_delete=models.CASCADE)
    checkFilesTags  = models.TextField(default='')
    checkExeRootTags = models.TextField(default='')
    checkPathTags = models.TextField(default='')
    checkKeyValueTags = models.TextField(default='')
    checkWindowTitleTags = models.TextField(default='')
    checkNWindowsTags = models.TextField(default='')
    checkDebugPrivsTags = models.TextField(default='')
    checkHdName = models.TextField(default='')
    checkTemp = models.TextField(default='')
    creation_date = models.DateTimeField(default=timezone.now)

    def __str__(self):
        return str(self.execution)
    

class OtherTools(models.Model):
    execution = models.ForeignKey(Execution, null=True, on_delete=models.CASCADE)
    pafish = models.TextField(default='')
    alkhaser = models.TextField(default='')
    creation_date = models.DateTimeField(default=timezone.now)

    def __str__(self):
        return str(self.execution)
