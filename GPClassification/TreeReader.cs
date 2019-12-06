using BraneCloud.Evolution.EC.GP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPClassification
{
    public static class TreeReader
    {
        static string code = "";
        public static string PrintCodeFromTree(GPTree tree)
        {
            code = "";
            var startNode = tree.Child;
            EvalNode(startNode);
            return code;

        }

        static void EvalNode(GPNode node)
        {
            switch (node.Name)
            {
                case "IF":
                    code += "if(";
                    EvalNode(node.Children[0]);
                    code += "){\n";
                    EvalNode(node.Children[1]);
                    code += "\n}\nelse\n{\n";
                    EvalNode(node.Children[2]);
                    code += "\n}";
                    break;
                case "AND":
                    code += "((";
                    EvalNode(node.Children[0]);
                    code += ") && (";
                    EvalNode(node.Children[1]);
                    code += "))";
                    break;
                case "OR":
                    code += "((";
                    EvalNode(node.Children[0]);
                    code += ") || (";
                    EvalNode(node.Children[1]);
                    code += "))";
                    break;
                case "NOT":
                    code += "!(";
                    EvalNode(node.Children[0]);
                    code += ")";
                    break;
                case "<":
                    EvalNode(node.Children[0]);
                    code += "<";
                    EvalNode(node.Children[1]);
                    break;
                case ">":
                    EvalNode(node.Children[0]);
                    code += ">";
                    EvalNode(node.Children[1]);
                    break;
                case "=":
                    EvalNode(node.Children[0]);
                    code += "==";
                    EvalNode(node.Children[1]);
                    break;
                case "ERC":
                    if (node.ToString() == "ERC(label)")
                        code += ("return \"" + node.ToStringForHumans() + "\";");
                    else
                        code += node.ToStringForHumans();
                    break;
                
                default:
                    throw new Exception("Nie zdefiniowano wezla");
                    

            }
        }
    }
}
