from ipwhois import IPWhois

def parse_ip(ip):
    results = None
    try:
        obj = IPWhois(ip.strip())
        results = obj.lookup_rdap(depth=1)
    except:
        print(" [-] Error getting ip info: <" + ip.strip() + ">")
        pass
    return results


def analyze_ip(ip):
    info = parse_ip(ip)
    d_info = {}
    if info is not None:
        if not 'asn_country_code' in info:
            asn_country_code = ''
        else:
            asn_country_code = info['asn_country_code']
        if not 'asn_description' in info:
            asn_description = ''
        else:
            asn_description = info['asn_description']

        d_info = {
            'ip': info['query'],
            'asn_country_code': asn_country_code,
            'asn_description':asn_description,
            'network': {
                'status': info['network']['status'],
                'start_address':info['network']['start_address'],
                'end_address':info['network']['end_address'],
                'cidr':info['network']['cidr'],
                'ip_version':info['network']['ip_version'],
                'name':info['network']['name'],
                'country':info['network']['country']
            }
        }
        for obj_k, obj_v in info['objects'].items():
            d_aux = {'address':'',
            'email':''}
            if 'contact' in obj_v and obj_v['contact'] is not None:
                tags = ['address', 'email']
                for t in tags:
                    if not t in obj_v['contact']:
                        obj_v['contact'][t] = ''
                d_aux = {
                    'address': obj_v['contact']['address'],
                    'email': obj_v['contact']['email']
                }
            d_info[obj_k] = d_aux
    return d_info