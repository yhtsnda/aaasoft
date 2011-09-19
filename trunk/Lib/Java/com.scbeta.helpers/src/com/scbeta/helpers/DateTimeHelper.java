/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.helpers;

import java.text.SimpleDateFormat;
import java.util.Calendar;
import java.util.Date;

/**
 *
 * @author aaa
 */
public class DateTimeHelper {

    public static Calendar getDayStart(Calendar calendar) {
        Calendar tmpCalendar = Calendar.getInstance();
        tmpCalendar.setTime(calendar.getTime());

        tmpCalendar.set(Calendar.HOUR_OF_DAY, 0);
        tmpCalendar.set(Calendar.MINUTE, 0);
        tmpCalendar.set(Calendar.SECOND, 0);
        return tmpCalendar;
    }

    public static Date getDayStart(Date day) {
        Calendar tmpCalendar = Calendar.getInstance();
        tmpCalendar.setTime(day);
        return getDayStart(tmpCalendar).getTime();
    }

    public static Calendar getDayEnd(Calendar calendar) {
        Calendar tmpCalendar = Calendar.getInstance();
        tmpCalendar.setTime(calendar.getTime());

        tmpCalendar.set(Calendar.HOUR_OF_DAY, 23);
        tmpCalendar.set(Calendar.MINUTE, 59);
        tmpCalendar.set(Calendar.SECOND, 59);
        return tmpCalendar;
    }

    public static Date getDayEnd(Date day) {
        Calendar tmpCalendar = Calendar.getInstance();
        tmpCalendar.setTime(day);
        return getDayEnd(tmpCalendar).getTime();
    }

    /**
     * 得到当天的开始时间
     */
    public static Calendar getTodayStartTime() {
        return getDayStart(Calendar.getInstance());
    }

    /**
     * 得到当天的开始时间字符串
     */
    public static String getTodayStartTimeString() {
        return GetDateTimeString(getTodayStartTime());
    }

    /**
     * 得到当天的结束时间
     * @return 
     */
    public static Calendar getTodayEndTime() {
        return getDayEnd(Calendar.getInstance());
    }

    /**
     * 得到当天的结束时间字符串
     */
    public static String getTodayEndTimeString() {
        return GetDateTimeString(getTodayEndTime());
    }

    // 获取当前日期与时间字符串
    public static String GetNowDateTime() {
        return GetDateTimeStringBase(Calendar.getInstance(), "yyyy-MM-dd HH:mm:ss");
    }

    // 获取当前日期与时间字符串(仅数字)
    public static String GetNowDateTimeNo() {
        return GetDateTimeStringBase(Calendar.getInstance(), "yyyyMMddHHmmss");
    }

    public static String GetDateTimeString(Calendar time) {
        return GetDateTimeStringBase(time, "yyyy-MM-dd HH:mm:ss");
    }

    public static String GetDateTimeStringBase(Calendar time, String formateString) {
        return GetDateTimeStringBase(time.getTime(), formateString);
    }

    public static String GetDateTimeStringBase(Date time, String formateString) {
        SimpleDateFormat dateFm = new SimpleDateFormat(formateString);
        return dateFm.format(time);
    }
}
