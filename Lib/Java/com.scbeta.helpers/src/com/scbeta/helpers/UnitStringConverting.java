/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.helpers;

import java.util.Arrays;
import java.util.List;

/**
 *
 * @author aaa
 */
public class UnitStringConverting {
    /// <summary>
    /// 单位字符串列表
    /// </summary>

    List<String> UnitList;
    /// <summary>
    /// 单位之间的换算比例列表
    /// </summary>
    List<Double> UnitsScaleList;

    public static UnitStringConverting getTimeUnitStringConverting() {
        String[] TimeUnitArray = new String[]{
            "毫秒", "秒", "分钟", "小时", "天"
        };
        Double[] TimeUnitConversionArray = new Double[]{
            1000D, 60D, 60D, 24D
        };
        try {
            return new UnitStringConverting(TimeUnitArray, TimeUnitConversionArray);
        } catch (Exception ex) {
            return null;
        }
    }

    public static UnitStringConverting getStorageUnitStringConverting() {
        String[] StorageUnitArray = new String[]{
            "", "K", "M", "G", "T", "P", "E", "Z", "Y"
        };
        Double StorageUnitConvert = 1024D;
        try {
            return new UnitStringConverting(StorageUnitArray, StorageUnitConvert);
        } catch (Exception ex) {
            return null;
        }
    }

    public UnitStringConverting(String[] UnitArray, Double UnitConversion) throws Exception {
        Double[] UnitConversionArray = null;
        if (UnitArray != null && UnitArray.length >= 2) {
            UnitConversionArray = new Double[UnitArray.length - 1];
            for (int i = 0; i <= UnitConversionArray.length - 1; i++) {
                UnitConversionArray[i] = UnitConversion;
            }
        }
        init(UnitArray, UnitConversionArray);
    }

    public UnitStringConverting(String[] UnitArray, Double[] UnitsScaleArray) throws Exception {
        init(UnitArray, UnitsScaleArray);
    }

    private void init(String[] UnitArray, Double[] UnitsScaleArray) throws Exception {
        if (UnitArray == null || UnitArray.length == 0 || UnitsScaleArray == null || UnitArray.length != UnitsScaleArray.length + 1) {
            throw new Exception("转换单位数组参数不正确");
        }
        this.UnitList = Arrays.asList(UnitArray);
        this.UnitsScaleList = Arrays.asList(UnitsScaleArray);
    }

    /// <summary>
    /// 得到指定最小单位数量的合适单位序号
    /// </summary>
    /// <param name="MinUnitNumber">最小单位数量</param>
    /// <returns></returns>
    public Integer getFitUnitIndex(Double MinUnitNumber) {
        Double TmpNumber = MinUnitNumber;
        for (int i = 0; i <= UnitsScaleList.size() - 1; i++) {
            Double tmpUnitConversion = UnitsScaleList.get(i);
            TmpNumber = TmpNumber / tmpUnitConversion;
            if (Math.abs(TmpNumber) < 1) {
                return i;
            }
        }
        return UnitList.size() - 1;
    }

    /// <summary>
    /// 得到指定最小单位数量的合适单位字符串
    /// </summary>
    /// <param name="MinUnitNumber">最小单位数量</param>
    /// <returns></returns>
    public String getFitUnitString(Double MinUnitNumber) {
        return UnitList.get(getFitUnitIndex(MinUnitNumber));
    }

    // 得到指定单位和数量的最小单位数量
    /// <summary>
    /// 得到指定单位和数量的最小单位数量
    /// </summary>
    /// <param name="UnitNumber">数量</param>
    /// <param name="UnitIndex">单位序号</param>
    /// <returns></returns>
    public Double getMinUnitUnits(Double UnitNumber, Integer UnitIndex) {
        Double TmpValue = UnitNumber;
        for (int i = UnitIndex - 1; i >= 0; i--) {
            TmpValue = TmpValue * UnitsScaleList.get(i);
        }
        return TmpValue;
    }

    /// <summary>
    /// 得到指定单位和数量的最小单位数量
    /// </summary>
    /// <param name="UnitNumber">数量</param>
    /// <param name="Unit">单位字符串</param>
    /// <returns></returns>
    public Double getMinUnitUnits(Double UnitNumber, String Unit) throws Exception {
        Integer UnitIndex = UnitList.indexOf(Unit);
        if (UnitIndex < 0) {
            throw new Exception("在单位列表中未找到指定的单位。");
        }
        return getMinUnitUnits(UnitNumber, UnitIndex);
    }

    /// <summary>
    /// 得到指定最小单位数量在指定单位的值
    /// </summary>
    /// <param name="MinUnitNumber">最小单位数量</param>
    /// <param name="UnitIndex">单位序号</param>
    /// <returns></returns>
    public Double getUnits(Double MinUnitNumber, Integer UnitIndex) {
        Double tmpValue = MinUnitNumber;
        for (int i = 0; i <= UnitIndex - 1; i++) {
            tmpValue = tmpValue / UnitsScaleList.get(i);
        }
        return tmpValue;
    }

    /// <summary>
    /// 得到指定最小单位数量在指定单位的值
    /// </summary>
    /// <param name="MinUnitNumber">最小单位数量</param>
    /// <param name="Unit">单位字符串</param>
    /// <returns></returns>
    public Double getUnits(Double MinUnitNumber, String Unit) throws Exception {
        Integer UnitIndex = UnitList.indexOf(Unit);
        if (UnitIndex < 0) {
            throw new Exception("在单位列表中未找到指定的单位。");
        }
        return getUnits(MinUnitNumber, UnitIndex);
    }

    /// <summary>
    /// 得到合适单位与指定小数点位数的最小单位数量转换为合适单位后的字符串
    /// </summary>
    /// <param name="MinUnitNumber">最小单位数量</param>
    /// <returns></returns>
    public String getString(Double MinUnitNumber) {
        return getString(MinUnitNumber, 0, false);
    }

    /// <summary>
    /// 得到合适单位与指定小数点位数的最小单位数量转换为合适单位后的字符串
    /// </summary>
    /// <param name="MinUnitNumber">最小单位数量</param>
    /// <param name="NumberOfDoublePlaces">小数点位数</param>
    /// <param name="SpaceBetweenNumberAndUnit">数字与单位之间是否加空格</param>
    /// <returns></returns>
    public String getString(Double MinUnitNumber, Integer NumberOfDoublePlaces, Boolean SpaceBetweenNumberAndUnit) {
        Integer UnitIndex = getFitUnitIndex(MinUnitNumber);
        return getString(MinUnitNumber, UnitIndex, NumberOfDoublePlaces, SpaceBetweenNumberAndUnit);
    }

    /// <summary>
    /// 得到指定单位与小数点位数的最小单位数量转换为指定单位后的字符串
    /// </summary>
    /// <param name="MinUnitNumber">最小单位数量</param>
    /// <param name="Unit">单位字符串</param>
    /// <param name="NumberOfDoublePlaces">小数点位数</param>
    /// <param name="SpaceBetweenNumberAndUnit">数字与单位之间是否加空格</param>
    /// <returns></returns>
    public String getString(Double MinUnitNumber, String Unit, Integer NumberOfDoublePlaces, Boolean SpaceBetweenNumberAndUnit) throws Exception {
        Integer UnitIndex = UnitList.indexOf(Unit);
        if (UnitIndex < 0) {
            throw new Exception("在单位列表中未找到指定的单位。");
        }
        return getString(MinUnitNumber, UnitIndex, NumberOfDoublePlaces, SpaceBetweenNumberAndUnit);
    }

    /// <summary>
    /// 得到指定单位与小数点位数的最小单位数量转换为指定单位后的字符串
    /// </summary>
    /// <param name="MinUnitNumber">最小单位数量</param>
    /// <param name="UnitIndex">单位序号</param>
    /// <param name="NumberOfDoublePlaces">小数点位数</param>
    /// <param name="SpaceBetweenNumberAndUnit">数字与单位之间是否加空格</param>
    /// <returns></returns>
    public String getString(Double MinUnitNumber, Integer UnitIndex, Integer NumberOfDoublePlaces, Boolean SpaceBetweenNumberAndUnit) {
        StringBuilder sb = new StringBuilder();

        Double unitUnits = getUnits(MinUnitNumber, UnitIndex);
        String unitUnitsString = String.format("%1$." + NumberOfDoublePlaces + "f", unitUnits);
        String UnitString = UnitList.get(UnitIndex);

        sb.append(unitUnitsString);
        if (SpaceBetweenNumberAndUnit) {
            sb.append(" ");
        }
        sb.append(UnitString);

        return sb.toString();
    }

    /// <summary>
    /// 得到合适单位到指定小单位或最小单位的最小单位数量转换为字符串
    /// </summary>
    /// <param name="MinUnitNumber">最小单位数量</param>
    /// <returns></returns>
    public String getString2(Double MinUnitNumber) {
        return getString2(MinUnitNumber, false);
    }

    /// <summary>
    /// 得到合适单位到指定小单位或最小单位的最小单位数量转换为字符串
    /// </summary>
    /// <param name="MinUnitNumber">最小单位数量</param>
    /// <param name="SpaceBetweenNumberAndUnit">数字与单位之间是否加空格</param>
    /// <returns></returns>
    public String getString2(Double MinUnitNumber, Boolean SpaceBetweenNumberAndUnit) {
        return getString2(MinUnitNumber, UnitList.size() - 1, 0, SpaceBetweenNumberAndUnit);
    }

    /// <summary>
    /// 得到合适单位到指定小单位或最小单位的最小单位数量转换为字符串
    /// </summary>
    /// <param name="MinUnitNumber">最小单位数量</param>
    /// <param name="MinUnitIndex">小单位序号</param>
    /// <param name="SpaceBetweenNumberAndUnit">数字与单位之间是否加空格</param>
    /// <returns></returns>
    public String getString2(Double MinUnitNumber, Integer MinUnitIndex, Boolean SpaceBetweenNumberAndUnit) {
        return getString2(MinUnitNumber, UnitList.size() - 1, MinUnitIndex, SpaceBetweenNumberAndUnit);
    }

    /// <summary>
    /// 得到合适单位到指定小单位或最小单位的最小单位数量转换为字符串
    /// </summary>
    /// <param name="MinUnitNumber">最小单位数量</param>
    /// <param name="MinUnit">小单位字符串</param>
    /// <param name="SpaceBetweenNumberAndUnit">数字与单位之间是否加空格</param>
    /// <returns></returns>
    public String getString2(Double MinUnitNumber, String MinUnit, Boolean SpaceBetweenNumberAndUnit) throws Exception {
        return getString2(MinUnitNumber, null, MinUnit, SpaceBetweenNumberAndUnit);
    }

    /// <summary>
    /// 得到合适单位到指定小单位或最小单位的最小单位数量转换为字符串
    /// </summary>
    /// <param name="MinUnitNumber">最小单位数量</param>
    /// <param name="MaxUnit">大单位字符串</param>
    /// <param name="MinUnit">小单位字符串</param>
    /// <param name="SpaceBetweenNumberAndUnit">数字与单位之间是否加空格</param>
    /// <returns></returns>
    public String getString2(Double MinUnitNumber, String MaxUnit, String MinUnit, Boolean SpaceBetweenNumberAndUnit) throws Exception {
        Integer MaxUnitIndex = UnitList.size() - 1;
        if (!(MaxUnit == null || MaxUnit.equals(""))) {
            MaxUnitIndex = UnitList.indexOf(MaxUnit);
            if (MaxUnitIndex < 0) {
                throw new Exception("在单位列表中未找到指定的大单位。");
            }
        }
        Integer MinUnitIndex = UnitList.indexOf(MinUnit);
        if (MinUnitIndex < 0) {
            throw new Exception("在单位列表中未找到指定的小单位。");
        }

        return getString2(MinUnitNumber, MaxUnitIndex, MinUnitIndex, SpaceBetweenNumberAndUnit);
    }

    /// <summary>
    /// 得到合适单位到指定小单位或最小单位的最小单位数量转换为字符串
    /// </summary>
    /// <param name="MinUnitNumber">最小单位数量</param>
    /// <param name="MaxUnitIndex">大单位序号</param>
    /// <param name="MinUnitIndex">小单位序号</param>
    /// <param name="SpaceBetweenNumberAndUnit">数字与单位之间是否加空格</param>
    /// <returns></returns>
    public String getString2(Double MinUnitNumber, Integer MaxUnitIndex, Integer MinUnitIndex, Boolean SpaceBetweenNumberAndUnit) {
        StringBuilder sb = new StringBuilder();

        Double TmpValue = MinUnitNumber;
        for (int i = MaxUnitIndex; i >= MinUnitIndex; i--) {
            Double CurrentUnitUnits = getUnits(TmpValue, i);
            String CurrentUnitUnitsString = Integer.toString(CurrentUnitUnits.intValue());
            String UnitString = UnitList.get(i);

            TmpValue = TmpValue - getMinUnitUnits(CurrentUnitUnits, i);

            if (CurrentUnitUnits > 0 || sb.length() > 0 || i == MinUnitIndex) {
                sb.append(CurrentUnitUnitsString);
                if (SpaceBetweenNumberAndUnit) {
                    sb.append(" ");
                }
                sb.append(UnitString);
                if (SpaceBetweenNumberAndUnit) {
                    sb.append(" ");
                }
            }
        }
        if (SpaceBetweenNumberAndUnit && sb.length() > 0) {
            sb.deleteCharAt(sb.length() - 1);
        }
        return sb.toString();
    }
}
