if (page == "none") then
	mes("warp02-00");
	menu("warp02-01","Go.","none","Nothing.");
end
if (page == "warp02-01") then
	if (hasregval("pop3guard")) then
		warp(3,8,248);
		mes("warp02-01");
		menu("none","Thank you!!.");
	else
		mes("warp02-02");
		menu("none","OK.");
	end
end