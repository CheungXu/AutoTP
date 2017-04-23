#include <fstream>
#include <sstream>
#include <vector>
#include <string>
#include <algorithm>
#include <iomanip>

/*****************************************************************************
* ��������
*	analysis
* ����������
*	-const std::string: ������׼���ݵ� .txt �ļ�·��
*	-const std::string: �������Եõ������ݵ� .txt �ļ���·��
*	-const std::string: ���������������ݵ� .txt �ļ���·��
* ����ֵ��
*	-int:
*		 0: ���гɹ�
*		-1: ��ȡ�ļ�·������
* ˵����
*	�˺�����ȡ��ȷ�������Լ����Եõ������ݣ������ó����ٻ��ʺͲ�׼��д�뵽��Ӧ
*	���ļ��С�
****************************************************************************/

int analysis(const std::string labels_path, const std::string results_path, const std::string save_path) {
	std::ifstream labels(labels_path);
	std::ifstream result(results_path);
	std::ofstream save(save_path);

	if (!labels || !result || !save)
		return -1;

	std::string standard_name, test_name;
	std::string standard_line, test_line;

	// ���� lambda �������������εľ��η�����������һ�����εĸ������̣��������ٻ��ʺͲ�׼��
	auto f = [](std::vector<std::vector<unsigned>> deno_vec, std::vector<std::vector<unsigned>> accu_vec) {
		double result = 0.0;
		for (const auto &deno : deno_vec) {
			std::vector<double> score;

			for (const auto &accu : accu_vec) {
				double ratio = 0.0;
				if (deno[0] > accu[2] || deno[2] < accu[0]
					|| deno[1] > accu[3] || deno[3] < accu[1])
					;
				else {
					std::vector<unsigned> x_vec{ deno[0], deno[2], accu[0], accu[2] };
					std::vector<unsigned> y_vec{ deno[1], deno[3], accu[1], accu[3] };

					std::sort(x_vec.begin(), x_vec.end());
					std::sort(y_vec.begin(), y_vec.end());

					double min_area, max_area;
					min_area = (x_vec[2] - x_vec[1] + 1) * (y_vec[2] - y_vec[1] + 1);
					max_area = (x_vec[3] - x_vec[0] + 1) * (y_vec[3] - y_vec[0] + 1);
					ratio = min_area / max_area;
				}
				score.push_back(ratio);
			}
			result += *std::max_element(score.cbegin(), score.cend());
		}
		result /= deno_vec.size();

		return result;
	};

	while (std::getline(labels, standard_line) && std::getline(result, test_line)) {
		std::vector<std::vector<unsigned>> standard_vec, test_vec;
		std::istringstream standard(standard_line), test(test_line);

		// ��ȡͼƬ���Ʋ��ж��Ƿ�����
		standard >> standard_name;
		test >> test_name;
		if (standard_name != test_name) {
			std::getline(result, test_line);
			test.str(test_line);
			test >> test_name;
		}

		// ��ȡ���β���
		unsigned x = 0, y = 0, width = 0, height = 0;
		while (standard >> x && standard >> y && standard >> width && standard >> height)
			standard_vec.push_back({ x, y, x + width - 1, y + height - 1 });
		while (test >> x && test >> y && test >> width && test >> height)
			test_vec.push_back({ x, y, x + width - 1, y + height - 1 });

		// ����Ŀ������
		double recall = 0.0, precision = 0.0;
		if (standard_vec.size() && test_vec.size()) {
			recall = f(standard_vec, test_vec);
			precision = f(test_vec, standard_vec);
		}
		save.precision(4);
		save.showpoint;
		save << standard_name << " "
			<< recall * 100 << " "
			<< precision * 100 << std::endl;
	}
	return 0;
}

int main() {
	// ·�����и���
	analysis("E:\\Temp\\labels.txt", "E:\\Temp\\results.txt", "E:\\Temp\\test.txt");
	return 0;
}