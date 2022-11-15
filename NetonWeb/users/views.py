import imp
from django.http import HttpResponseRedirect

from django.shortcuts import render
from django.contrib.auth import logout as do_logout
from django.contrib.auth import login as do_login
from django.contrib.auth import authenticate
# from django.contrib.auth.forms import AuthenticationForm
from .forms import AuthenticationForm
import logging

log_info = logging.getLogger('core.info')
log_error = logging.getLogger('core.error')

# login functions
def login(request):
    form = AuthenticationForm()
    if request.method == "POST":
        form = AuthenticationForm(data=request.POST)
        if form.is_valid():
            username = form.cleaned_data['username']
            password = form.cleaned_data['password']
            user = authenticate(username=username, password=password)
            if user is not None:
                log_info.info("[+] User " + username + ': logged')
                do_login(request, user)
                return HttpResponseRedirect('/')
            else:
                log_error.error("[+] User " + username + ': authentication failed')
    return render(request, "users/login.html", {'form': form})


def logout(request):
    # Finalizamos la sesión
    do_logout(request)
    # Redireccionamos a la portada
    return HttpResponseRedirect('/users/login')
