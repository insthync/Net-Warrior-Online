if (page == "none") then
	mes("warp00-00");
	menu("warp00-01","Go.","none","Nothing.");
end
if (page == "warp00-01") then
	warp(1,108,20);
	mes("warp00-01");
	menu("none","Thank you!!.");
end