if (page == "none") then
	mes("in03-00");
	menu("in03-01","Type_of_Signal","in03-02","Type_of_Signal_Transmission","none","Nothing.");
end
if (page == "in03-00") then
	mes("in03-00");
	menu("in03-01","Type_of_Signal","in03-02","Type_of_Signal_Transmission","none","Nothing.");
end
if (page == "in03-01") then
	mes("in03-01");
	menu("in03-00","OK");
end
if (page == "in03-02") then
	mes("in03-02");
	menu("in03-00","OK");
end