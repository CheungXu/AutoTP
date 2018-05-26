# coding = utf-8

import re
try:
    import xml.etree.cElementTree as ET
except ImportError:
    import xml.etree.ElementTree as ET

# rec := [x,y,width,height]
# box := [type,x,y,width,height]
# data_set := {num1:[box]}

def read_txt(txt_path,txt_name):
    """
    	no type info in txt
    	so add 'txt_name' param
    """
    ret_data_set = {"ALL_SUM":0}
    with open(txt_path,'r') as txt_file:
        for each_row in txt_file.readlines():

            each_row = each_row.strip('\n')

            if not len(each_row) <= 1:
                ret_data_set["ALL_SUM"] += 1
                temp_list = each_row.split(' ')
                temp_frame = int(temp_list.pop(0).strip(".jpg"))

                ret_data_set[temp_frame] = []

                temp_rec = list(map(int,temp_list))
                temp_rec.insert(0,txt_name)
                ret_data_set[temp_frame].append(temp_rec)

    return ret_data_set


def read_xml(xml_path):
    ret_data_set = {"ALL_SUM":0}
    number_compile = re.compile(r'\d+')
    try:
        ret_XML = ET.parse(xml_path)
        root = ret_XML.getroot()
    except Exception as e:
        ret_XML = open(xml_path).read()
        ret_XML = re.sub(u"[\x00-\x08\x0b-\x0c\x0e-\x1f]+",u"",ret_XML)

        root = ET.fromstring(ret_XML)
    else:
        pass
    finally:
        pass

    for ch in root:
        # print(ch.tag)
        # type(ch.tag) # str
        if ch.tag.endswith("Number"):
            ret_data_set["ALL_SUM"] += int(ch.text)
            number_frame = int(number_compile.findall(ch.tag)[0])
            ret_data_set[number_frame] = []
            continue
        else:
            one_data = []
            for data in ch:
                if data.tag == "Type":
                    one_data.append(data.text.strip('"'))
                elif data.tag == "Position":

                    rec_p = re.split(r'\s',data.text.strip())

                    rec_p = list(map(int,rec_p))
                    one_data.extend(rec_p)
                    number_frame = int(number_compile.findall(ch.tag)[0])
                    ret_data_set[number_frame].append(one_data)
    return ret_data_set


def test_test():

    test_xml_path = ".//test_data//程序结果//01T形交叉_下-Result.xml"

    ret_list = read_xml(test_xml_path)
    print(ret_list)

    test_txt_path = ".//test_data//标注结果//01T形交叉_下//labels.txt"
    DS = read_txt(test_txt_path,'01T形交叉_下')
    print(DS)

    print("Done")
def get_chart():
    with open("sign_name_chart.txt",'r') as chart_file:
        chart_file.read()
if __name__ == '__main__':
    # test_test()

    get_chart()
