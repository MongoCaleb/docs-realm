// :code-block-start: define-object-model-local
package com.mongodb.realm.examples.model;

public enum TaskStatus {
    Open("Open"),
    InProgress("In Progress"),
    Complete("Complete");

    String displayName;
    TaskStatus(String displayName) {
        this.displayName = displayName;
    }
}
// :code-block-end: