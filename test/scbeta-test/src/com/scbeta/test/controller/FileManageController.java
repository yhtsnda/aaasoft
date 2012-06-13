package com.scbeta.test.controller;

import java.io.File;
import java.util.ArrayList;
import java.util.Date;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import javax.servlet.ServletContext;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;

import org.springframework.stereotype.Controller;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;

@Controller
public class FileManageController {
	@RequestMapping(value = "/fileList.do", method = RequestMethod.GET)
	public String fileList_get(HttpServletRequest request,
			HttpServletResponse response, String path) {
		ServletContext servletContext = request.getSession()
				.getServletContext();
		// WEB根路径
		String webRootPath = servletContext.getRealPath("");
		// contextPath;
		String contextPath = servletContext.getContextPath();
		request.setAttribute("contextPath", contextPath);
		// 当前请求的URI
		String requestURI = request.getRequestURI();

		List<Map<String, Object>> fileInfoList = new ArrayList<Map<String, Object>>();

		if (path == null || path.isEmpty()) {
			path = "";
		}
		File file = new File(webRootPath + path);
		// 如果路径不存在
		if (file.exists()) {
			if (file.isDirectory()) {
				for (File subFile : file.listFiles()) {
					// 文件名称
					Map<String, Object> fileInfo = new HashMap<String, Object>();
					fileInfo.put("name", subFile.getName());

					// 文件短路径
					String fileShortPath = subFile.getPath()
							.substring(webRootPath.length()).replace("\\", "/");

					// 类型
					String fileType = "未知";
					try {
						fileType = (subFile.isFile()) ? "文件" : "目录";
					} catch (Exception ex) {
					}
					fileInfo.put("fileType", fileType);

					if ("文件".equals(fileType)) {
						fileInfo.put("url", contextPath + fileShortPath);
						try {
							fileInfo.put("length", subFile.length());
						} catch (Exception ex) {
							ex.printStackTrace();
							fileInfo.put("length", "");
						}
					} else if ("目录".equals(fileType)) {
						fileInfo.put("url", requestURI + "?path="
								+ fileShortPath);
						fileInfo.put("length", "");
					}
					// 修改时间
					try {
						fileInfo.put("lastModified",
								new Date(subFile.lastModified()).toString());
					} catch (Exception ex) {
						fileInfo.put("lastModified", "未知");
					}

					fileInfoList.add(fileInfo);
				}
			} else {
				path = "路径[" + path + "]指向了一个文件";
			}
		} else {
			path = "未找到路径[" + path + "]";
		}

		if (path == "") {
			path = "/";
		}
		request.setAttribute("path", path);
		System.out.println(file.getParent());
		System.out.println(webRootPath);

		String parentPath = file.getParent();
		if (parentPath.equals(webRootPath)) {
			parentPath = "/";
		} else if (parentPath.length() > webRootPath.length()) {
			parentPath = file.getParent().substring(webRootPath.length())
					.replace("\\", "/");
		} else {
			parentPath = null;
		}
		request.setAttribute("parentPath", parentPath);
		request.setAttribute("fileInfoList", fileInfoList);
		return "fileList";
	}
}
