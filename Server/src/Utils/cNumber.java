package Utils;

public class cNumber {
	public static boolean isNumber(final String str) {
		for (int i = 0; i < str.length(); i++) {
			//If we find a non-digit character we return false.
			if (!Character.isDigit(str.charAt(i)))
				return false;
		}
		return true;
	}
}
