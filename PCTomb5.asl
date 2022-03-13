state("PCTomb5")
{
    int level : 0xA5C2D0;
    bool isLoading : 0xA5BF60;
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
    return current.level == 0 && current.isLoading && !old.isLoading;
}

split
{
    if(old.level != 0) {
        if(current.level > old.level)
        {
            return true;
        }
        if(current.level == 0 && old.level == 14) // end
        {
            return true;
        }
    }
    return false;
}
