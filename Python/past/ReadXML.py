# -*- coding: utf-8 -*-
try:
    import xml.etree.cElementTree as ET
except ImportError:
    import xml.etree.ElementTree as ET

def RecJudge(correctRec, findRec):
	"""
		Rec :: [type,x,y,width,height]
	"""

	if correctRec[0] == findRec[0]:
		findRec_center = [findRec[1] + 0.5 * findRec[3],findRec[2] + 0.5 * findRec[4]]
		correctRec_range = [correctRec[1]-correctRec[3],
							correctRec[1]+correctRec[3],
							correctRec[2]-correctRec[4],
							correctRec[2]+correctRec[4]]
		# print correctRec_range
		if correctRec_range[0]<=findRec_center[0]<=correctRec_range[1] and correctRec_range[2]<=findRec_center[1]<=correctRec_range[3]:
			return True
	return False

def ReadXML(XMLPath):
	theList = {"ALL_SUM":0}
	correct_XML = ET.parse(XMLPath)
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

	# Right Number
	n_TP = 0

	# 标准答案标记：
	correct_List = ReadXML("Correct_Sign.xml")
	#print correct_List
	# 找到的标记
	find_List = ReadXML("Find_Sign.xml")
	#print find_List

	correct_keys = list(filter(lambda x : type(x) == int, correct_List.keys()))
	find_keys = list(filter(lambda x : type(x) == int, find_List.keys()))

	for item_number in correct_keys:
		if item_number in find_keys:
			for correct_item in correct_List[item_number]:
				for find_item in find_List[item_number]:
					if RecJudge(correct_item,find_item) == True:
						n_TP += 1
	n_FP = find_List["ALL_SUM"] - n_TP
	n_FN = correct_List["ALL_SUM"] - n_TP

	Precision = n_TP / (n_TP + n_FP)
	Recall = n_TP / (n_TP + n_FN)
	F_measure = (2*Precision*Recall)/(Precision+Recall)


	print("n_TP = ", n_TP)
	print("n_FP = ", n_FP)
	print("n_FN = ", n_FN)
	print("Precision = ", Precision)
	print("Recall = ", Recall)
	print("F_measure = ", F_measure)