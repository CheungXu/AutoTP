# coding = utf-8

try:
    import xml.etree.cElementTree as ET
except ImportError:
    import xml.etree.ElementTree as ET

# rec := [x,y,width,height]
# box := [type,x,y,width,height]
# data_set := {num1:[box]}

def read_txt(txt_path):
    pass


def read_xml(xml_path):
    theList = {"ALL_SUM":0}
	correct_XML = ET.parse(xml_path)
	root = correct_XML.getroot()

	for ch in root:
		# print ch.tag
		if ch.tag.endswith("Number"):
			theList["ALL_SUM"] += int(ch.text)
			theList[int(ch.tag[5:10])] = []
			continue
		else:
			one_data = []
			for data in ch:
				if data.tag == "Type":
					one_data.append(data.text)
				elif data.tag == "Position":
					rec_p = data.text.split(" ")
					rec_p = list(map(int,rec_p))
					one_data.extend(rec_p)
			theList[int(ch.tag[5:10])].append(one_data)

	return theList


if __name__ == '__main__':
    pass
