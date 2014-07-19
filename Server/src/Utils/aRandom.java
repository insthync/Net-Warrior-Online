package Utils;

public class aRandom {
	public static int doRandom(int aStart, int aEnd) {
		// random entity
	    int randomNumber =  (int)Math.floor(Math.random()*(1+aEnd-aStart))+aStart;
	    return randomNumber;
	}
}
