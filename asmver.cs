using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

public class AssemblyVersion
{
	public static void Main(string[] args)
	{
		IEnumerable<string> paths = args;
		var verbose = false;
		
		if (args[0] == "-v") {
			paths = args.Skip(1);
			verbose = true;
		}
		
		foreach (var path in paths) {
			try {
				var asm = Assembly.LoadFrom(path);
				Console.Write(path + ": " + asm.FullName);

				var infoVersion = (AssemblyInformationalVersionAttribute)asm.GetCustomAttributes(typeof(AssemblyInformationalVersionAttribute), false).FirstOrDefault();
				if (verbose && infoVersion != null)
				{
					Console.Write(" (product version: {0})", infoVersion.InformationalVersion);
				}

				Console.WriteLine();

				if (verbose && asm.GetName().GetPublicKey() != null && asm.GetName().GetPublicKey().Length > 0) {
					Console.Write("public key: ");
					var pubkey = asm.GetName().GetPublicKey();
					for (int i=0;i<pubkey.GetLength(0);i++) {
 						Console.Write ("{0:x2}", pubkey[i]);
 					}
 					Console.WriteLine();
				}
				if (verbose) {
					foreach (var reference in asm.GetReferencedAssemblies()) {
						Console.WriteLine("\treference: " + reference.FullName);
					}
				}
			} catch (Exception e) {
				while (e != null) {
					Console.Error.WriteLine(e.Message);
					e = e.InnerException;
				}
			}
		}
	}
}
