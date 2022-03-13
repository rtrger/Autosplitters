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
    return current.level == 1 && !current.isLoading && old.isLoading;
}

reset
{
    return current.level == 1 && current.isLoading && !old.isLoading;
}

split
{
    if(!current.isLoading && old.isLoading)
    {
	if(current.level == 3  || // tomb of seth
           current.level == 5  || // valley of the kings
           current.level == 7  || // temple of karnak
           current.level == 11 || // tomb of semerkhet
           current.level == 13 || // desert railroad
           current.level == 14 || // alexandria
           current.level == 22 || // city of the dead

           current.level == 27 || // citadel
           current.level == 28 || // sphinx complex (twice)
           current.level == 37)   // temple of horus
        {
            return true;
        }
    }
    if(current.level == 0 && old.level == 38) // end
    {
        return true;
    }
    return false;
}
