#include "rc.h"

PLACE labels_place  = "E:\\Temp\\test\\labels.txt";
PLACE result_place  = "E:\\Temp\\test\\results.txt";
PLACE save_place	= "E:\\Temp\\test\\TestAnalysis.txt";

int main(int argc, char *argv[]) {
	// 根据输入更改文件路径
	switch (argc) {
	case 2:
		labels_place = argv[1] + labels_place;
		result_place = argv[1] + result_place;
		save_place   = argv[1] + save_place;
		break;
	case 5:
		labels_place = PLACE(argv[1]) + argv[2];
		result_place = PLACE(argv[1]) + argv[3];
		save_place   = PLACE(argv[1]) + argv[4];
		break;
	default:
		break;
	}

	std::list<CmpResult> cmp_result;
	readAndCompute(labels_place, result_place, cmp_result);
	save(save_place, cmp_result);

	return 0;
}