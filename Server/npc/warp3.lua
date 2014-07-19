if (page == "none") then
	mes("warp03-00");
	menu("warp03-01","Go.","none","Nothing.");
end
if (page == "warp03-01") then
	if (hasregval("ftpguard")) then
		warp(4,248,248);
		mes("warp03-01");
		menu("none","Thank you!!.");
	else
		mes("warp03-02");
		menu("none","OK.");
	end
end