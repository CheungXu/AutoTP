# -*- coding: utf-8 -*- 

Labels_path = 'E:\\TrafficSignData\\Testing\\labels.txt'
Result_path = 'E:\\TrafficSignData\\Testing\\results.txt'

TestAnalysis_path = 'E:\\TrafficSignData\\Testing\\TestAnalysis.txt'


# --- read txt and make dir -------
def make_dir(txt):
    txt_dir = {}
    for eachLine in txt.readlines():

        temp = eachLine.split(' ')

        first_letter = temp[0].split('.')
        keey = int(first_letter[0])
        txt_dir[keey] = []
        del temp[0]
        while (len(temp) != 0):
            
            plist = temp[:4]

            try:
                plist = map(int,plist)
            except ValueError:
                break

            txt_dir[keey].append(plist)

            del temp[:4]

    return txt_dir

# -------- get area --------
def get_area(thing):
    theArea = thing[2] * thing[3]
    return theArea

# -------- mp ----------
def mp(label,result):
    
    x_list = [label[0],label[0]+label[2],result[0],result[0]+result[2]]
    y_list = [label[1],label[1]+label[3],result[1],result[1]+result[3]]
    x_list.sort()
    y_list.sort()
    # 矩形的交面积
    theAnd =float( (x_list[2]-x_list[1]) * (y_list[2]-y_list[1]))
    # 矩形的并面积
    theOr = float((x_list[3]-x_list[0]) * (y_list[3]-y_list[0]))

    rate = float( theAnd / theOr )

    return rate

# ---------- MAIN ----------

Out_txt = open(TestAnalysis_path,'w')

Labels_txt = open(Labels_path,'r')
Result_txt = open(Result_path,'r')

Labels_dir = {}
Result_dir = {}

Labels_dir = make_dir(Labels_txt)
Result_dir = make_dir(Result_txt)

out_list = {}

Labels_txt.close()
Result_txt.close()

tempout_list =[]
tlist = []
mR = []


sortList = list(Result_dir.keys())
sortList.sort()

for key in sortList:
    if (key in Labels_dir and len(Result_dir[key]) != 0):
        # ------- Precision -------
        for ii in range(len(Result_dir[key])):
            for iii in range(len(Labels_dir[key])):
                mprate = mp(Labels_dir[key][iii],Result_dir[key][ii])
                tempout_list.append(mprate)
            tlist.append((max(tempout_list)))
        
        Precision = int( float(sum(tlist)/len(Result_dir[key])) * 100)
        tempout_list = []
        tlist = []
        # -------- Recall --------
        for iii in range(len(Labels_dir[key])):
            for ii in range(len(Result_dir[key])):
                mprate = mp(Labels_dir[key][iii],Result_dir[key][ii])
                tempout_list.append(mprate)
            
            tlist.append((max(tempout_list)))
        
        Recall = int( float((sum(tlist)/len(Labels_dir[key]))) * 100)
        tempout_list = []
        tlist = []
        # ---- write ---------
        pic = str(key) + '.jpg' + ' '
        Out_txt.write(pic)
        data = str(Recall)+' '+str(Precision)
        Out_txt.write(data)
        Out_txt.write('\n')

        tempout_list =[]

    else:
        pic = str(key) + '.jpg' + ' '
        Out_txt.write(pic)
        data = '0'+' '+'0'+' '
        Out_txt.write(data)
        Out_txt.write('\n')

Out_txt.close()

Result_dir.clear()
Labels_dir.clear()

print 'Over'

