# coding = utf-8

import os
from analysis_data import *
from read_file import *

PRINT_EMPTY = False

def main():
	# current path
	cur_path = os.path.abspath('.')
	# print(cur_path)

	'''
	# get path list
	cur_file_list = os.listdir(cur_path)
	print(cur_file_list)
	'''

	cur_data_path = os.path.join(cur_path,'test_data')
	# print(cur_data_path)

	# ================= the correct file path=================
	correct_file_path = os.path.join(cur_data_path,"correct")
	# print(correct_file_path)

	correct_file_list = os.listdir(correct_file_path)
	# print(correct_file_list)
	# ========================================================

	target_file_list = correct_file_list.copy()
	# print(target_file_list)

	# ================ the find file path ===================
	find_file_path = os.path.join(cur_data_path,"find")
	# print(find_file_path)
	find_file_list = os.listdir(find_file_path)
	# print(find_file_list)
	# =======================================================
	result_txt_path = os.path.join(cur_path,'autotp_result.txt')
	# print(result_txt_path)
	total_tp = 0
	total_fp = 0
	total_fn = 0
	temp_list = []
	with open(result_txt_path,'w') as write_file:
		for cur_item in target_file_list:
			# print(cur_item)

			correct_txt_path = os.path.join(correct_file_path,cur_item)
			temp_txt_name = os.listdir(correct_txt_path)[0]
			correct_txt_path = os.path.join(correct_txt_path,temp_txt_name)
			# print(correct_txt_path)

			find_xml_name = cur_item+'-Result.xml'
			find_xml_path = os.path.join(find_file_path,find_xml_name)
			# print(find_xml_path)
			correct_data_set = read_txt(correct_txt_path,cur_item)
			find_data_set = read_xml(find_xml_path)

			(p,r,f,tp,fp,fn) = analysis_data(find_data_set,correct_data_set)
			total_fn += fn
			total_fp += fp
			total_tp += tp
			if PRINT_EMPTY or int(p*r*f*1000000) >= 1:
				temp_list.append(p)
				write_line = ret_write_line(cur_item,p,r,f)
				# print(write_line)
				write_file.writelines(write_line)
			del find_data_set
			del correct_data_set
		total_p = total_tp/(total_tp+total_fp)
		total_r = total_tp/(total_tp+total_fn)
		total_f = (2*total_p*total_r)/(total_p+total_r)
		write_line = ret_write_line('TOTAL',total_p,total_r,total_f)
		write_file.writelines(write_line)
	print(temp_list)
	print("Average = "+str(sum(temp_list)/8))
	return None

def ret_write_line(name,Precision,Recall,F_measure):
	ret_line = name + "\n\tPrecision:" + str(Precision) + '\n\tRecall:'+ str(Recall) +'\n\tF_measure:' + str(F_measure) + '\n\n'
	return ret_line

def test_main():
	test_xml_path = ".//test_data//find//01T形交叉_下-Result.xml"
	ret_list = read_xml(test_xml_path)
	# print(ret_list)
	test_txt_path = ".//test_data//correct//01T形交叉_下//labels.txt"
	DS = read_txt(test_txt_path,'01T形交叉_下')
	# print(DS)
	(a,b,c) = analysis_data(ret_list,DS)
	# print(a,b,c)

if __name__ == '__main__':
	main()
	# test_main()