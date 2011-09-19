/*
 * To change this template, choose Tools | Templates
 * and open the template in the editor.
 */
package com.scbeta.helpers;

import java.io.UnsupportedEncodingException;
import java.security.MessageDigest;
import java.security.NoSuchAlgorithmException;
import java.util.logging.Level;
import java.util.logging.Logger;

/**
 *
 * @author aaa
 */
public class CryptographyHelper {

    public static String computeMD5Hash(String data) {
        byte[] sourceData;
        try {
            sourceData = data.getBytes("utf-8");
        } catch (UnsupportedEncodingException ex) {
            Logger.getLogger(CryptographyHelper.class.getName()).log(Level.SEVERE, null, ex);
            return null;
        }

        byte[] resultData = computeMD5Hash(sourceData);
        if (resultData == null) {
            return null;
        }
        StringBuilder sb = new StringBuilder();
        for (byte b : resultData) {
            sb.append(String.format("%02X", b));
        }
        return sb.toString().toLowerCase();
    }

    public static byte[] computeMD5Hash(byte[] data) {
        try {
            MessageDigest md5 = MessageDigest.getInstance("MD5");
            md5.update(data);
            return md5.digest();
        } catch (NoSuchAlgorithmException ex) {
            Logger.getLogger(CryptographyHelper.class.getName()).log(Level.SEVERE, null, ex);
        }
        return null;
    }
}
