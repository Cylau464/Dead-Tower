using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public struct Rewards
{
    public int Gold;
    public int Exp;
    public Resource[] Resources;

    public Rewards(int gold, int exp, Resource[] resources)
    {
        Gold = gold;
        Exp = exp;
        Resources = resources;
    }

    public static Rewards operator +(Rewards a, Rewards b)
    {
        List<Resource> resources = a.Resources?.ToList();

        if (resources == null)
        {
            resources = b.Resources?.ToList();
        }
        else if(b.Resources != null)
        {
            foreach(Resource res in b.Resources)
            {
                Resource firstRes = resources.FirstOrDefault(x => x.Type == res.Type);

                if (firstRes == default(Resource))
                    resources.Add(res);
                else
                    resources[resources.IndexOf(firstRes)] += res;
            }
        }

        if (resources == null)
            resources = new List<Resource>();

        return new Rewards(a.Gold + b.Gold, a.Exp + b.Exp, resources.ToArray());
    }
}
