# coding = utf-8

# rec := [x,y,width,height]
# box := [type,x,y,width,height]
# frame := [box,box,box]
# data_set := {'ALL_SUM':**,frame:[box,box,...]}

def iscross(find_rec, correct_rec):
    # rec := [x,y,width,height]
    t_p1 = [max(find_rec[0], correct_rec[0]),
            max(find_rec[1], correct_rec[1])]

    t_p2 = [
            min(find_rec[0] + find_rec[2],
                correct_rec[0] + correct_rec[2]
                ),
            min(find_rec[1] + find_rec[3],
                correct_rec[1] + correct_rec[3]
                )
            ]

    if t_p1[0] <= t_p2[0] and t_p1[1] <= t_p2[1]:
        return True
    else:
        return False

def get_IoU(find_box, correct_box):
    # is cross
    if iscross(find_box[1:], correct_box[1:]) == True:
        x_list = [
                find_box[1],
                find_box[1] + find_box[3],
                correct_box[1],
                correct_box[1] + correct_box[3]
                ]
        y_list = [
                find_box[2],
                find_box[2] + find_box[4],
                correct_box[2],
                correct_box[2] + correct_box[4]
                ]

        x_list.sort()
        y_list.sort()

        cross_area = (x_list[2] - x_list[1]) * (y_list[2] - y_list[1])
        union_area = find_box[3] * find_box[4] + \
            correct_box[3] * correct_box[4] - cross_area

        the_iou = cross_area / union_area

        return the_iou
    else:
        return 0

def isgrade(find_box,correct_box):
    # box := [type,x,y,width,height]
    #
    if find_box[0] == correct_box[0]:

        find_center = [find_box[1]+0.5*find_box[3],find_box[2]+0.5*find_box[4]]

        if correct_box[1]<=find_center[0]<=correct_box[1]+correct_box[3] and \
         correct_box[2]<=find_center[1]<=correct_box[2]+correct_box[4]:
            return True
        else:
          return False
    else:
        return False

def analysis_one_frame(find_frame,correct_frame):
    # frame := [box,box,box]
    # return n_TP
    one_n_TP = 0
    for one_correct_box in correct_frame:
        for one_find_box in find_frame:
            if isgrade(one_find_box,one_correct_box) == True:
                one_n_TP += 1
            else:
                continue
    return one_n_TP

def analysis_data(find_data,correct_data):
    # data_set := {'ALL_SUM':**,frame:[box,box,...]}

    find_all_sum = find_data.pop('ALL_SUM')
    correct_all_sum = correct_data.pop('ALL_SUM')
    n_TP = 0

    frame_list = list(correct_data.keys())
    frame_list.sort()

    for one_frame in frame_list:
        n_TP += analysis_one_frame(find_data[one_frame],correct_data[one_frame])

    n_FP = abs(find_all_sum - n_TP)
    n_FN = abs(correct_all_sum - n_TP)

    try:
        Precision = n_TP / (n_TP + n_FP)
    except ZeroDivisionError as e:
        Precision = 0.0
    try:
        Recall = n_TP / (n_TP + n_FN)
    except ZeroDivisionError as e:
        Recall = 0.0
    try:
        F_measure = (2 * Precision * Recall) / (Precision + Recall)
    except ZeroDivisionError as e:
        F_measure = 0.0
    if Precision*Recall == 0:
        n_FN = 0
        n_FP = 0
        n_TP = 0
    return (Precision,Recall,F_measure,n_TP,n_FP,n_FN)


if __name__ == '__main__':
    pass
