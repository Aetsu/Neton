from django.urls import path

from . import views

app_name = 'core'
urlpatterns = [
    path('', views.get_executions, name='get_executions'),
    path('info/<str:executionId>', views.get_execution, name='get_execution'),
    path('get_executions_json', views.get_executions_json, name='get_executions_json'),
    path('collect_data', views.collect_data, name='collect_data'),
    path('hooks', views.get_hooks, name='get_hooks'),
    path('filecrawler', views.get_filecrawler, name='get_filecrawler'),
    path('systeminfo', views.get_systeminfo, name='get_systeminfo'),
    path('checksandbox', views.get_checksandbox, name='get_checksandbox'),
    path('sharpedrchecker', views.get_sharpedrchecker, name='get_sharpedrchecker'),
    path('alkhaser', views.get_alkhaser, name='get_alkhaser'),
    path('pafish', views.get_pafish, name='get_pafish'),
    path('export_json', views.export_json, name='export_json'),
]
