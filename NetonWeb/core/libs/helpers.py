import base64
import json
import os
import random
import string
from pathlib import Path

def id_generator(size=16, chars=string.ascii_lowercase):
    return ''.join(random.choice(chars) for _ in range(size))


def check_folder(foldername):
    """
    Checks if a folder exists
    """
    if not os.path.exists(foldername):
        os.makedirs(foldername)


def check_file(filename):
    """
    Checks if a file exists
    """
    if not os.path.isfile(filename):
        return False
    else:
        return True


def remove_empty_lists(tuples):
    """
    Delete empty lists in a list of lists
    """
    tuples = [t for t in tuples if t]
    return tuples


def remove_empty_elements(l):
    """
    Delete empty elements in a list
    """
    return list(filter(None, l))


def sort_unique(l):
    """
    Sort unique elements of a list
    """
    aux = set(l)
    return list(aux)

def sort_unique_l_dicts(l):
    return [dict(p) for p in set(tuple(i.items()) 
        for i in l)]



def strip_list_files(l):
    return [x.strip() for x in l]


def list_diff(list1, list2):
    return (list(set(list1) - set(list2)))


def chunk_list(l, n):
    n = max(1, n)
    return list(l[i:i+n] for i in range(0, len(l), n))


def b64encode(data):
    # return base64.b64encode(data)
    response = None
    if data is not None:
        try:
            data_str = base64.b64encode(data.encode('utf-8', errors='strict'))
            response = data_str.decode('utf-8')
        except:
            return response
    return response


def b64decode(data):
    response = ''
    if data is not None:
        data_str = base64.b64decode(data.encode('utf-8', errors='strict'))
        return data_str.decode('utf-8')
    else:
        return ''


def b64encode_file(input_file):
    response = None
    if check_file(input_file):
        try:
            with open(input_file, "rb") as image_file:
                data_str = base64.b64encode(image_file.read())
                response = data_str.decode('utf-8')
        except:
            pass
    return response

def b64decode_file(b64_str, output_file):
    status = False
    try:
        with open(output_file, "wb") as image_file:
            image_file.write(base64.b64decode(b64_str))
        status = True
    except:
        pass
    return status

def read_json_file(file_path):
    try:
        my_file = Path(file_path)
        if my_file.is_file():
            with open(file_path) as json_data:
                parsed_json = json.load(json_data)
        else:
            parsed_json = {}
    except:
        parsed_json = {}
        pass
    return parsed_json


def write_json_file(d_content, file_path):
    with open(file_path, 'w') as fp:
        json.dump(d_content, fp)


def list_files(folder):
    l_content = []
    for file in os.listdir(folder):
        l_content.append(os.path.join(folder, file))
    return l_content


def parse_file_content(file_path):
    content_files = list_files(file_path)
    l_content = []
    for f in content_files:
        with open(f, 'r', errors='ignore') as rf:
            content = rf.read()
        l_content += content.split('\n')
        os.remove(f)
    l_content = strip_list_files(l_content)
    l_content = remove_empty_elements(l_content)
    l_content = sort_unique(l_content)
    return l_content


def pretty_print_json(json_obj):
    print(json.dumps(json_obj, indent=4, sort_keys=True))


def add_to_file(elem, output_file):
    with open(output_file, 'a') as wf:
        wf.write(elem + '\n')


def read_from_file(input_file):
    with open(input_file, 'r') as rf:
        content = rf.read()
    l_hosts = content.split('\n')
    l_hosts = remove_empty_elements(l_hosts)
    return l_hosts


def remove_html_tags(text):
    """Remove html tags from a string"""
    import re
    clean = re.compile('<.*?>')
    return re.sub(clean, '', text)
