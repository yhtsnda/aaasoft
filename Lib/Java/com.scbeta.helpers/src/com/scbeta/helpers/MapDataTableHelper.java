/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.helpers;

import java.util.List;
import java.util.Map;

/**
 *
 * @author aaa
 */
public class MapDataTableHelper {

    // 得到行数据
    public static int GetRowCount(Map<String, List<String>> map) {
        int rowCount = -1;
        for (String columnName : map.keySet()) {
            List<String> columnData = map.get(columnName);
            if (rowCount == -1) {
                rowCount = columnData.size();
            } else {
                if (rowCount > columnData.size()) {
                    rowCount = columnData.size();
                }
            }
        }
        return rowCount;
    }
}
