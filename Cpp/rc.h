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
 * �����
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
 * ������Σ�������ε����ϽǺ����½Ƕ���
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
 * ���屣��������Ľṹ�壬����ͼƬ���ƣ���׼�ʺ��ٻ���
 */
struct CmpResult {
	std::string picture;
	double	recall;
	double	precision;

	CmpResult(std::string _picture, std::pair<double, double> _result) :
		picture(_picture), recall(_result.first), precision(_result.second) {}
};

/*
 * ��������
 *	readAndCompute
 * �����б����ͣ�
 *	- const PLACE: Labels.txt �ľ���·��
 *	- const PLACE: Result.txt �ľ���·��
 *	- list<CmpResult> &:	������������ list ����
 * ����ֵ��
 *	- bool: ���������Ƿ�õ���Ҫ�Ľ��
 * ˵����
 *	�ú�����ȡ txt Դ�ļ����������������������㣬���������������� list ������
 */
bool readAndCompute(const PLACE labels_place, const PLACE result_place, std::list<CmpResult> &cmp_result);

/*
 * ��������
 *	readRect
 * �����б����ͣ�
 *	- istringstream &: �õ����β����� istringstream ��
 *	- vector<Rectangle> &: �����ȡ���ľ��ν���� vector ����
 * ����ֵ��
 *	- bool: ���������Ƿ�õ���Ҫ�Ľ��
 * ˵����
 *	�ú�����ȡ istringstream ��������ȡ���� Rectangle ���浽 vector ������
 */
bool readRect(std::istringstream &input, std::vector<Rectangle> &rec_vec);

/*
 * ��������
 *	compute
 * �����б����ͣ�
 *	- const vector<Rectangle> &: ������ȷ��ע���ε���Ϣ
 *	- const vector<Rectangle> &: ������Ա�ע���ε���Ϣ
 * ����ֵ��
 *	- pair<unsigned, unsigned>: ����õ����ٻ��ʺͲ�׼��
 * ˵����
 *	�ú�����ȡ������ע������������ٻ��ʺͲ�׼��
 */
std::pair<double, double> compute(const std::vector<Rectangle> &standard_vec, const std::vector<Rectangle> &test_vec);

/*
 * ��������
 *	GetResult
 * �����б����ͣ�
 *	- const std::vector<Rectangle> &: ���ѭ���е� vector
 *	- const std::vector<Rectangle> &: �ڲ�ѭ���е� vector
 * ����ֵ��
 *	- double: ����ľ��η������
 * ˵����
 *	�ú�����ȡ��һ�� vector �о��εľ��η������֮��
 */
double getResult(const std::vector<Rectangle> &accu_vec, const std::vector<Rectangle> &deno_vec);

/*
 * ��������
 *	compareArea
 * �����б����ͣ�
 *	- const Rectangle &: ���ڱȽϵľ���
 *	- const Rectangle &: ���ڱȽϵľ���
 * ����ֵ��
 *	- double: ����ľ��η������
 * ˵����
 *	�ú�������������εľ��η������
 */
double compareArea(const Rectangle &rec1, const Rectangle &rec2);

/*
 * ��������
 *	save
 * �����б����ͣ�
 *	- const PLACE: �������ı���·��
 *	- const list<CmpResult> &: ������������ list ����
 * ����ֵ��
 *	- bool: ���������Ƿ�õ���Ҫ�Ľ��
 * ˵����
 *	�ú���������������ָ��·����
 */
bool save(const PLACE save_place, const std::list<CmpResult> &cmp_result);

#endif