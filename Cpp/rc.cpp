#include "rc.h"

bool readAndCompute(const PLACE labels_place, const PLACE result_place, std::list<CmpResult> &cmp_result) {
    std::ifstream labels(labels_place);
    std::ifstream result(result_place);

    if (!labels || !result) {
        std::cout << "读取文件出错，请检查文件路径是否正确。" << std::endl;
        system("pause");
        return false;
    }

    std::string standard_name, test_name;
	std::string standard_line, test_line;
	// 每次循环同步读取 labels.txt 和 results.txt 的每一行
	while (std::getline(labels, standard_line) && std::getline(result, test_line)) {
		// 读取图片名称并判断是否相等
		std::vector<Rectangle> standard_vec, test_vec;
		std::istringstream standard(standard_line), test(test_line);
		standard >> standard_name;
		test	 >> test_name;

		if (standard_name != test_name) {
			std::cout << standard_name << " 和 " << test_name << " 不匹配，源文件中这两张图片信息处于同一行。" << std::endl;
			system("pause");
			return false;
		}

		// 读取矩形参数
		readRect(standard, standard_vec);
		readRect(test, test_vec);

		// 根据读取的矩形求目标参数
		cmp_result.push_back(CmpResult(standard_name, compute(standard_vec, test_vec)));
	}

	return true;
}

bool readRect(std::istringstream &input, std::vector<Rectangle> &rec_vec)
{
	unsigned x = 0, y = 0, width = 0, height = 0;
	while (input >> x && input >> y && input >> width && input >> height) {
		rec_vec.push_back(Rectangle(x, y, width, height));
	}

	return true;
}

std::pair<double, double> compute(const std::vector<Rectangle> &standard_vec, const std::vector<Rectangle> &test_vec) {
	if (standard_vec.size() == 0 || test_vec.size() == 0)
		return{ 0.0, 0.0 };

	double recall, precision;
	recall = getResult(standard_vec, test_vec) / standard_vec.size();
	precision = getResult(test_vec, standard_vec) / test_vec.size();

	return std::make_pair(recall, precision);
}

double getResult(const std::vector<Rectangle> &deno_vec, const std::vector<Rectangle> &accu_vec) {
	double result = 0.0;

	if (!deno_vec.size() || !accu_vec.size())
		return result;
	else
		for (const auto &deno : deno_vec) {
			std::vector<double> score;

			for (const auto &accu : accu_vec)
				score.push_back(compareArea(accu, deno));

			result += *std::max_element(score.cbegin(), score.cend());
		}

	return result;
}

double compareArea(const Rectangle & rec1, const Rectangle & rec2)
{
	if (rec1.lu.x > rec2.rb.x || rec1.rb.x < rec2.lu.x
		|| rec1.lu.y > rec2.rb.y || rec1.rb.y < rec2.lu.y)
		return 0.0;

	std::vector<unsigned> x_vec{ rec1.lu.x, rec1.rb.x, rec2.lu.x, rec2.rb.x };
	std::vector<unsigned> y_vec{ rec1.lu.y, rec1.rb.y, rec2.lu.y, rec2.rb.y };

	std::sort(x_vec.begin(), x_vec.end());
	std::sort(y_vec.begin(), y_vec.end());

	double min_area, max_area;
	min_area = (x_vec[2] - x_vec[1] + 1) * (y_vec[2] - y_vec[1] + 1);
	max_area = (x_vec[3] - x_vec[0] + 1) * (y_vec[3] - y_vec[0] + 1);

	return min_area / max_area;
}

bool save(const PLACE save_place, const std::list<CmpResult>& cmp_result)
{
	std::ofstream out(save_place);

	if (!out) {
		std::cout << "写入错误，请检查写入路径是否正确。" << std::endl;
		system("pause");
		return 0;
	}

	for (const auto &element : cmp_result)
		out << element.picture << " " << (int)(element.recall * 100) << " " << (int)(element.precision * 100) << "\n";

	return true;
}
