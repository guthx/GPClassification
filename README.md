# GPClassification
An application based on genetic programming used to generate classifiers based on 2 different structures:
1. Single-tree classifier, similar to decision trees. Build of nested IF instructions, that compare data attributes to constants.
2. Multi-classifier, built of a set of smaller programs for each class, that return TRUE or FALSE depending on whether a data record belongs to a given class or not. These programs are then sorted based on their effectivenes and launched until a TRUE value is returned (or there's no programs left).
