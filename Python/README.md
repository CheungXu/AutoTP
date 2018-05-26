# .//Past//
之前写的程序，可供参考

# analysis_data.py == 未验证
数据分析脚本，计算比赛要求的查准率，查全率和F-measure
同时提供 **IoU** 值的计算

# read_file.txt == 未验证
读取文件，生成规定格式的数据，可读XML文件

# 数据结构格式
1. ** rec := [x,y,width,height] **
2. ** box := [type,x,y,width,height] **
3. ** frame := [box,box,box] **
4. ** data_set := {'ALL_SUM':--,frame:[box,box,...]} **
