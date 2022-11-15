from django.contrib import admin

from .models import Execution, Targets, Hook, Checksandbox, Filecrawler, Systeminfo

admin.site.register(Targets)
admin.site.register(Execution)
admin.site.register(Hook)
admin.site.register(Checksandbox)
admin.site.register(Filecrawler)
admin.site.register(Systeminfo)