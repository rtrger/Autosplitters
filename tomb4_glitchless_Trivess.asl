state("tomb4")
{
    int level : 0x3FD290;
    bool isLoading : 0x1333A8;
}

init
{
    print("Process found!");
    refreshRate = 30;
}

start
{
	if(current.level == 1 && !current.isLoading && old.isLoading){
		vars.levelPointer = 1;
		vars.pointerCambodia = 1;
		vars.pointerKarnak = 1;
		vars.pointerAlexandria = 1;
		vars.pointerLostLibrary = 1;
		vars.pointerTempleOfIsis = 1;
		vars.pointerCairo = 1;
		vars.pointerGiza = 1;
		return true;
	}
    return false;
}

reset
{
	if(current.level == 0 && !current.isLoading){
		vars.levelPointer = 0;
		vars.pointerCambodia = 0;
		vars.pointerKarnak = 0;
		vars.pointerAlexandria = 0;
		vars.pointerLostLibrary = 0;
		vars.pointerTempleOfIsis = 0;
		vars.pointerCairo = 0;
		vars.pointerGiza = 0;
		return true;
	}
    return false;
}

split
{
	var nextLevel = vars.levelPointer+1;
    print("current level" +  current.level);
    print("levelPointer" + vars.levelPointer);

    // NORMAL LEVELS
    if(
		nextLevel == current.level
		&& current.level != 2  //CAMBODIA LEVELS
		&& current.level != 8 && current.level != 9  //KARNAK LEVELS
		&& current.level != 15 && current.level != 18 // ALEXANDRIA LEVELS
		&& current.level != 16 && current.level != 17 // LIBRARY Levels
		&& current.level != 19 && current.level != 20 //TEMPLE LEVELS
		&& current.level != 23 && current.level != 24 && current.level != 25 && current.level != 26 //CAIRO LEVELS
		&& current.level != 28 && current.level != 30 && current.level != 31 && current.level != 32 //GIZA LEVELS
		&& current.level != 38 // FIRST HORUS LEVEL
    )
    {
        vars.levelPointer++;
        return true;
    }
	if(
        //SEQUENCE LEVEL CAMBODIA
		(vars.pointerCambodia == 1 && old.level == 1 && current.level == 2)//RACE
    ){
		vars.pointerCambodia++;
		vars.levelPointer = 2;
		return false;
	}
    if(
        //SEQUENCE LEVEL KARNAK
		(vars.pointerKarnak == 1 && old.level == 7 && current.level == 8) || //KARNAK TO HYPOSTOLE
		(vars.pointerKarnak == 2 && old.level == 8 && current.level == 9) || //HYPOSTOLE TO SACRED LAKE SPLIT
		(vars.pointerKarnak == 3 && old.level == 9 && current.level == 7) || //SACRED LAKE TO KARNAK SPLIT
		(vars.pointerKarnak == 4 && old.level == 7 && current.level == 8) || //KARNAK TO HYPOSTOLE SPLIT
		(vars.pointerKarnak == 5 && old.level == 8 && current.level == 9) //HYPOSTOLE TO SACRED LAKE SPLIT
    ){
		bool split = false;
		if(vars.pointerKarnak == 2 || vars.pointerKarnak == 3 || vars.pointerKarnak == 4 || vars.pointerKarnak == 5){
			split = true;
		}
		if(vars.pointerKarnak == 5){
			vars.levelPointer = 10;
		}
        vars.pointerKarnak++;
        return split;
    }
	
	if(
        //SEQUENCE LEVEL ALEXANDRIA
		(vars.pointerAlexandria == 1 && old.level == 14 && current.level == 15) || // ALEXANDRIA TO COASTAL
		(vars.pointerAlexandria == 2 && old.level == 15 && current.level == 18) || // COASTAL TO CATACOMBS
		(vars.pointerAlexandria == 3 && old.level == 18 && current.level == 15) || // CATACOMBS TO COASTAL
		(vars.pointerAlexandria == 4 && old.level == 15 && current.level == 18) || // COASTAL TO CATACOMBS
		(vars.pointerAlexandria == 5 && old.level == 18 && current.level == 15) || // CATACOMBS TO COASTAL
		(vars.pointerAlexandria == 6 && old.level == 15 && current.level == 18) || // COASTAL TO CATACOMBS SPLIT
		(vars.pointerAlexandria == 7 && old.level == 18 && current.level == 19) // CATACOMBS TO POSEIDON SPLIT
    ){
		bool split = false;
		if(vars.pointerAlexandria == 6 || vars.pointerAlexandria == 7){
			split = true;
		}
		if(vars.pointerAlexandria == 6){
			vars.levelPointer = 18;
		}
        vars.pointerAlexandria++;
		return split;
    }
	
	if(
        //SEQUENCE LEVEL LOST LIBRARY
		(vars.pointerLostLibrary == 1 && old.level == 19 && current.level == 20) || // POSEIDON TO LOST LIBRARY SPLIT
		(vars.pointerLostLibrary == 2 && old.level == 20 && current.level == 19)  // LOST LIBRARY TO POSEIDON SPLIT
    ){
		vars.pointerLostLibrary++;
		return true;
    }
	if(
        //SEQUENCE LEVEL TEMPLE OF ISIS
		(vars.pointerTempleOfIsis == 1 && old.level == 16 && current.level == 17) || // ISIS TO CLEOPATRA
		(vars.pointerTempleOfIsis == 2 && old.level == 17 && current.level == 16) || // CLEOPATRA TO ISIS
		(vars.pointerTempleOfIsis == 3 && old.level == 16 && current.level == 17) // ISIS TO CLEOPATRA SPLIT
    ){
		bool split = false;
		if(vars.pointerTempleOfIsis == 3){
			vars.levelPointer = 21;
			split = true;
		}
		vars.pointerTempleOfIsis++;
		return split;
    }
	
	if(
        //SEQUENCE LEVEL CAIRO
		(vars.pointerCairo == 1 && old.level == 22 && current.level == 24) || // City to thulun SPLIT
		(vars.pointerCairo == 2 && old.level == 24 && current.level == 23) || // thulun to trenches
		(vars.pointerCairo == 3 && old.level == 23 && current.level == 25) || // trenches to street bazaar SPLIT
		(vars.pointerCairo == 4 && old.level == 25 && current.level == 23) || // street bazaar to trenches 
		(vars.pointerCairo == 5 && old.level == 23 && current.level == 25) || // trenches to street bazaar
		(vars.pointerCairo == 6 && old.level == 25 && current.level == 23) || // street bazaar to trenches SPLIT
		(vars.pointerCairo == 7 && old.level == 23 && current.level == 26) || // trenches to citadel gate
		(vars.pointerCairo == 8 && old.level == 26 && current.level == 27) // citadel gate to citadel SPLIT
    ){
		bool split = false;
		if(vars.pointerCairo == 1 || vars.pointerCairo == 3 || vars.pointerCairo == 6 || vars.pointerCairo == 8){
			split = true;
		}
		if(vars.pointerCairo == 8){
			vars.levelPointer = 27;
		}
		vars.pointerCairo++;
		return split;
    }
	if(
		//SEQUENCE LEVEL GIZA
		(vars.pointerGiza == 1 && old.level == 27 && current.level == 28) || // citadel to Sphinx complex SPLIT
		(vars.pointerGiza == 2 && old.level == 28 && current.level == 30) || // Sphinx complex to underneath the sphinx SPLIT
		(vars.pointerGiza == 3 && old.level == 30 && current.level == 31) || // underneath the sphinx to MENKAURES PYRAMID SPLIT
		(vars.pointerGiza == 4 && old.level == 31 && current.level == 32) || // MENKAURES PYRAMID to INSIDE MENKAURES PYRAMID SPLIT
		(vars.pointerGiza == 5 && old.level == 32 && current.level == 28) // INSIDE MENKAURES PYRAMID to Sphinx complex
    ){
		bool split = false;
		if(vars.pointerGiza == 1 || vars.pointerGiza == 2 || vars.pointerGiza == 3 || vars.pointerGiza == 4){
			split = true;
		}
		if(vars.pointerGiza == 5){
			vars.levelPointer = 32;
		}
		vars.pointerGiza++;
		return split;
	}
    //END LEVEL
    if(current.level == 0 && old.level == 38)
    {
        return true;
    }
    return false;
}
//LEVELS
//Cambodia Training Levels
//Level 1: Angkor Wat 
//Level 2: Race for the Iris
//Valley of the Kings Levels
//Level 3: The Tomb of Seth
//Level 4: Burial Chambers
//Level 5: Valley of the Kings
//Level 6: KV5
//KARNAK
//Level 7: Temple of Karnak
//Level 8: Great Hypostyle Hall
//Level 9: Sacred Lake
//Level 10: ????????????????
//LEVEL 11: SEMERKHET
//LEVEL 12: GUARDIAN
//ALEXANDRIA
//Level 13: DESERTRAIL
//Level 14: ALEXANDRIA
//Level 15: Coastal Ruins
//Level 16: Pharos, Temple of Isis
//Level 17: Cleopatra's Palaces
//Level 18: Catacombs
//Level 19: Temple of Poseidon
//LOST LIBRARY
//Level 20: The Lost Library
//Level 21: Hall of Demetrius
//CAIRO
//Level 22: City of the Dead
//Level 23: Trenches
//Level 24: Chambers of Tulun
//Level 25: STREET BAZAAR
//Level 26: Citadel Gate
//Level 27: Citadel
//GIZA
//Level 28: SPHINX COMPLEX
//Level 29: ????????????????
//Level 30: UNDERNEATH SPHINX
//Level 31: MENKAURES PYRAMID
//Level 32: INSIDE MENKAURES PYRAMID
//Level 33: MASTABAS
//Level 34: GREAT PYRAMID
//Level 35: KUFUS QUEENS PYRAMID
//Level 36: INSIDE THE GREAT PYRAMID
//Level 37: INSIDE TEMPLE OF HORUS
//Level 38: TEMPLE OF HOURS BATTLE
//CHECK LEVEL