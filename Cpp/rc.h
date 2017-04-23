#include <iostream>
#include <fstream>
#include <sstream>
#include <vector>
#include <string>
#include <list>
#include <utility>
#include <algorithm>

#ifndef _RC_H_
#define _RC_H_

typedef std::string PLACE;

/*
 * 定义点
 */
struct RPoint {
	unsigned x;
	unsigned y;

	RPoint(unsigned _x, unsigned _y) :
		x(_x), y(_y) {}
	RPoint() :
		RPoint(0, 0) {}
};

/*
 * 定义矩形，保存矩形的左上角和右下角顶点
 */
struct Rectangle {
	RPoint lu;
	RPoint rb;

	Rectangle(RPoint _lu, RPoint _rb) :
		lu(_lu), rb(_rb) {}
	Rectangle(RPoint point, unsigned width, unsigned height) :
		lu(point), rb(point.x + width, point.y + height) {}
	Rectangle(unsigned x, unsigned y, unsigned width, unsigned height) :
		lu(x, y), rb(x + width, y + height) {}
};

/*
 * 定义保存计算结果的结构体，保存图片名称，查准率和召回率
 */
struct CmpResult {
	std::string picture;
	double	recall;
	double	precision;

	CmpResult(std::string _picture, std::pair<double, double> _result) :
		picture(_picture), recall(_result.first), precision(_result.second) {}
};

/*
 * 函数名：
 *	readAndCompute
 * 参数列表类型：
 *	- const PLACE: Labels.txt 的绝对路径
 *	- const PLACE: Result.txt 的绝对路径
 *	- list<CmpResult> &:	保存运算结果的 list 容器
 * 返回值：
 *	- bool: 函数运行是否得到想要的结果
 * 说明：
 *	该函数读取 txt 源文件，调用其他函数进行运算，并将运算结果保存在 list 容器中
 */
bool readAndCompute(const PLACE labels_place, const PLACE result_place, std::list<CmpResult> &cmp_result);

/*
 * 函数名：
 *	readRect
 * 参数列表类型：
 *	- istringstream &: 得到矩形参数的 istringstream 流
 *	- vector<Rectangle> &: 保存读取到的矩形结果的 vector 容器
 * 返回值：
 *	- bool: 函数运行是否得到想要的结果
 * 说明：
 *	该函数读取 istringstream 流并将读取到的 Rectangle 保存到 vector 容器中
 */
bool readRect(std::istringstream &input, std::vector<Rectangle> &rec_vec);

/*
 * 函数名：
 *	compute
 * 参数列表类型：
 *	- const vector<Rectangle> &: 保存正确标注矩形的信息
 *	- const vector<Rectangle> &: 保存测试标注矩形的信息
 * 返回值：
 *	- pair<unsigned, unsigned>: 运算得到的召回率和查准率
 * 说明：
 *	该函数读取两个标注结果，并计算召回率和查准率
 */
std::pair<double, double> compute(const std::vector<Rectangle> &standard_vec, const std::vector<Rectangle> &test_vec);

/*
 * 函数名：
 *	GetResult
 * 参数列表类型：
 *	- const std::vector<Rectangle> &: 外层循环中的 vector
 *	- const std::vector<Rectangle> &: 内层循环中的 vector
 * 返回值：
 *	- double: 求出的矩形分配分数
 * 说明：
 *	该函数求取第一个 vector 中矩形的矩形分配分数之和
 */
double getResult(const std::vector<Rectangle> &accu_vec, const std::vector<Rectangle> &deno_vec);

/*
 * 函数名：
 *	compareArea
 * 参数列表类型：
 *	- const Rectangle &: 用于比较的矩形
 *	- const Rectangle &: 用于比较的矩形
 * 返回值：
 *	- double: 求出的矩形分配分数
 * 说明：
 *	该函数求出两个矩形的矩形分配分数
 */
double compareArea(const Rectangle &rec1, const Rectangle &rec2);

/*
 * 函数名：
 *	save
 * 参数列表类型：
 *	- const PLACE: 运算结果的保存路径
 *	- const list<CmpResult> &: 保存运算结果的 list 容器
 * 返回值：
 *	- bool: 函数运行是否得到想要的结果
 * 说明：
 *	该函数保存运算结果到指定路径中
 */
bool save(const PLACE save_place, const std::list<CmpResult> &cmp_result);

#endif