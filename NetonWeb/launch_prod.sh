gunicorn -w 8 -b 0.0.0.0:8443 --certfile=certs/server.crt --keyfile=certs/server.key NetonWeb.wsgi --access-logfile logs/gunicorn_access.log
