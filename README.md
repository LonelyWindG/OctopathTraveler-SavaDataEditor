# 八方旅人存档编辑器

[![GitHub release](https://img.shields.io/github/v/release/LonelyWindG/OctopathTraveler-SaveDataEditor?style=for-the-badge)](https://github.com/LonelyWindG/OctopathTraveler-SaveDataEditor/releases/latest)
[![GitHub all releases](https://img.shields.io/github/downloads/turtle-insect/OctopathTraveler/total?style=for-the-badge&color=00B000)](https://github.com/LonelyWindG/OctopathTraveler-SaveDataEditor/releases)
[![GPLv3 license](https://img.shields.io/github/license/LonelyWindG/OctopathTraveler-SaveDataEditor?style=for-the-badge&color=blue)](https://github.com/LonelyWindG/OctopathTraveler-SaveDataEditor/blob/master/LICENSE)
[![Windows](https://img.shields.io/badge/PLATFORM-Windows-blueviolet?style=for-the-badge)](https://dotnet.microsoft.com/zh-cn/apps/desktop)
[![.NET 6.0](https://img.shields.io/badge/.NET-6.0-%234122AA?style=for-the-badge)](https://dotnet.microsoft.com/zh-cn/download/dotnet/6.0)

## 概要

适用于Steam, Xbox PC, Nintendo Switch等平台的八方旅人游戏存档

## 语言

[日本語](README_JA.md) [English](README_EN.md)

## 游戏官网

https://octopathtraveler.nintendo.com/

http://www.jp.square-enix.com/octopathtraveler/

## 运行要求

* Windows操作系统
* [.NET 桌面运行时 6.0](https://dotnet.microsoft.com/download), [点此下载](https://aka.ms/dotnet/6.0/windowsdesktop-runtime-win-x64.exe)
* 八方旅人存档文件
  * Steam存档路径`%USERPROFILE%/Documents/My Games/Octopath_Traveler/(数字编号)/SaveGames`, 在SaveGames文件夹里SaveData0对应着游戏的自动存档. SaveData1/2/3对应相应顺序的存档
  * Xbox PC存档路径`%LOCALAPPDATA%/Packages/39EA002F.FrigateMS_n746a19ndrrjg/SystemAppData/wgs/(一串字母数字)/(一串字母数字)`, 在该文件夹内大小为1000多或2000多KB的几个文件为每个存档位的存档文件;要快速找到要修改的存档, 可以通过进入游戏内保存一个存档, 此目录内修改日期最近的那个就是
  * Nintendo Switch, 从机器中导入导出存档

## Build环境

* Windows 10 (64位)
* Visual Studio 2022

## 新增/修改功能说明

* 快捷按钮(从左到右):
  * 打开存档按钮
    * 非只读模式下, 打开存档后时自动在程序当前目录下的"OctopathTraveler Backup"文件夹备份打开的这个存档, 备份文件全名为"打开的存档名称(包含扩展名)-打开时的年月日时分"
  * 保存按钮, 覆盖修改后的数据到打开的存档
    * 只读模式下不会显示保存按钮, 并且不会运行保存修改(启动时带有-readonlyMode参数, 本次启动会进入只读模式)
  * 另存为Json按钮, 把打开的存档数据导出为Json文件
  * 将存档转换为Json按钮, 选择指定路径的存档并选择一个导出路径, 将存档导出为Json保存到该路径
* 预先说明, 下列说明中提到的"坏档", 指存档修改后可能出现以下情况:
  * 存档无法正常载入
  * 存档可正常载入, 但游玩时游戏需要读取修改过的数据时出错(闪退等)
  * 请慎重修改存档
+ 1.基础数据: 显示一些存档基础数据及存档内保存的成就进度数据
  * 成就进度内的道具数量不能修改, 因为存档内保存数据并非单纯的一个数字, 而是保存的每件道具是否已获取, 所以无从修改(该条鼠标移动过去会显示每个类型道具的具体数量)
  * 不在成就进度内显示的其他涉及到进度的例如需要识破弱点的381个怪物, 是没有单独保存识破数量在存档内的
+ 2.角色: 显示角色的数据及穿戴装备
  * 角色数据: 显示等级经验当前血蓝职业副职业JP点等
    * 额外增加属性: 后面带+的数据为额外增加数据, 游戏内通过吃对应坚果获得; 几个特殊属性: ACC+, EVA+, CON+, AGI+分别对应命中, 回避, 会心, 速度
    * Exp为当前总经验, RawHP/MP为当前血蓝(非最大), 不确定当前总经验修改超过下一等级总经验及当前血蓝修改超过最大血蓝是否会坏档
    * FirstJob/SecondJob, 主/副职业都不建议修改, 主职业修改, 副职业如果将两个角色设置成同一个副职业, 修改后原副职业的装备未卸下, 副职业未解锁, 等这些操作是否会导致坏档未知, 所以如果需要修改建议在游戏内改
  * 角色装备: 显示角色当前穿戴的装备, 装备右侧按钮为修改当前穿戴装备
    * 该项同样不建议修改, 例如穿戴了一个未拥有的装备, 多个角色穿戴的同一个装备加一起超过了背包中的拥有数量等是否会导致坏档未知
* 3.道具
  * 左侧为背包道具显示, 第一列为道具ID, 数量右侧按钮为选择道具将该条道具替换为另一个道具
    * 前几项ID为0的表示该条数据是空的, 新增未拥有的道具可修改这些条目; 
    * 存档内会出现两个相同ID的道具条目(一般情况下两个数量不一致), 若要修改数量可进游戏查看实际数量找到对应那条
    * 如果没有ID对应的道具名称时(背包内不会出现或者有出现但是我不知道ID对应名称是哪个), 会显示道具ID(0x十六进制ID)
  * 右侧为全部道具的拥有状态, 打钩的为当前拥有的, 用于道具全收集对照
    * 此处只显示道具的当前拥有状态, 例如之前有过的但是卖掉的道具或任务完成用掉的任务道具, 只要不在背包内, 此处都会显示未拥有
    * 手动修改此区域的复选框, 不会对存档做任何修改, 但是会影响筛选的显示
    * 筛选按钮功能为: 在"显示所有", "显示未拥有", "显示已有"三种显示方式中循环切换
    * 导出可将当前列表显示的导出为文本文件, 便于自行在外部对照自己未拥有过的道具
  * 在左侧之间新增未拥有的道具, 能否用于解锁成就未知
    * 并且在此新增未拥有的道具, 是否会导致"再次获得该道具时, 成就统计内的道具数量不增加"也未知
    * 如果要这样修改并且要解锁成就, 可自行观察成就统计内的道具数量有没有增加, 如果没增加那估计是无法解锁成就
* 4.地点
  * 显示地点是否已到达过(有些地点游戏内会到达, 但是我不知道这个ID对应的地点名字是什么)
  * 179-185的7个基础职业祠堂, 我不确定顺序是不是对的(因为原作者只给了一个猎人祠堂的ID并且我添加其他祠堂时我自己存档已经全解锁过了, 目前是按游戏内的职业顺序排的), 有可能是按祠堂所属地区顺序排的(地区顺序看该页面的ID顺序)
* 5.捕获的魔物
  * 猎人的捕获魔物, 显示魔物名称, 技能剩余次数, 有没有启用(这个不知道干嘛的, 存档内有这个)
  * 该页面功能没有在原作者基础上进行修改
* 6.怪物弱点
  * 该页面目前只显示了成就需要的381个怪物, 没有其他例如Boss(及召唤物), NPC的弱点显示
  * ID左侧复选框表示是否已识破该怪物全部弱点, 修改会同步勾选或取消勾选右侧全部弱点
  * 弱点复选框为灰色表示该怪物没有这个弱点(改实际是可以改的, 但是我不知道改了会不会有问题, 而且改了也没意义所以就屏蔽掉了)
  * 标题栏的ID, 单击会显示当前识破及未识破的怪物数量, 右击可进行显示筛选
  * "战略家"成就可通过修改这里获得, 已验证的可行方式为: 
    * 保留一个未全识破的怪物, 其余怪物全部勾选, 进入游戏遇到该怪物然后识破弱点即可完成
    * 勾选全部怪物保存存档然后进游戏能否之间解锁成就未知
* 7.队伍
  * 该页面功能没有在原作者基础上进行修改
* 8.任务
  * 该页面功能没有在原作者基础上进行修改
* 9.宝箱&隐藏道具状态: 显示地点的宝箱隐藏道具获取数量
  * 标题栏的总和, 单击可显示收集进度, 右击可进行显示筛选
  * VALUE的含义可查看这里[八方旅人宝箱状态对照说明 - 百度贴吧](https://tieba.baidu.com/p/7822253075)
  * 此处显示内容无法进行修改, 如果错过宝箱或隐藏道具但想完成成就, 可在成就进度那里对应栏增加你错过的数量

## 游戏数据表

* 程序内嵌有info.xlsx及info_xxx.xlsx(xxx为语言代码)Excel数据表, 保存程序内显示的道具, 地点等的名称及一些额外数据
* 程序运行时可在文件菜单导出内嵌的数据表用于查看和编辑, 导出时会导出当前语言的info_xxx.xlsx数据表及info.xlsx英文数据表, 内嵌的没有当前语言数据表或当前为英文时只会导出info.xlsx
* 程序运行时会优先读取程序运行目录的info_xxx.xlsx或info.xlsx(没有当前语言的info_xxx.xlsx时读取), 没有时读取内嵌的数据表; 所以导出到当前运行目录的数据表请勿在不了解具体结构及读取规则的前提下随意修改, 否则可能会导致加载数据表时报错或某些数据为读取到不显示
* 快捷按钮栏右侧会显示当前加载的数据表名称及是否为内嵌数据表

## 特别感谢

* https://gbatemp.net/threads/octopath-traveler-save-editing.511125/
  * [SleepyPrince](https://gbatemp.net/members/sleepyprince.94652/)
  * [Takumah](https://gbatemp.net/members/takumah.456165/)
  * [Translate English by gen212](https://github.com/gen212/OctopathTraveler)
  * [SUDALV92](https://github.com/SUDALV92/OctopathTraveler-TreasureChests-)
* [八方旅人全成就指南 - Steam社区](https://steamcommunity.com/sharedfiles/filedetails/?id=2795091350)
* [八方旅人全资源清单 - Google表格](https://docs.google.com/spreadsheets/d/14Kz5mTAYdxqdgjbkbotAMGC2aoiJBbrBUiLeh8Pwu0Q)
* [八方旅人宝箱状态对照表 - Google表格](https://docs.google.com/spreadsheets/d/1WGN0166crI5IbnJ4QADnLiNHrL2FUr0MVFqmWH7dBRg)
* [八方旅人宝箱状态对照说明 - 百度贴吧](https://tieba.baidu.com/p/7822253075)
* [八方旅人怪物ID对照表 - Google表格](https://docs.google.com/spreadsheets/d/1O1OYHmLNsUcak5dByXbmEFDaxIbp-mDSHGC6j92P5ho)
* [【成就心得】學者 X 戰略家、收藏家、滴水不漏](https://forum.gamer.com.tw/C.php?bsn=31593&snA=585)
* [GVAS游戏存档转换工具](https://github.com/januwA/gvas-converter)
