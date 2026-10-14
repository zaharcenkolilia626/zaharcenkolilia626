package bolts;

public class TaskCompletionSource<TResult> {
    private final Task<TResult> taskIk8kmh98claminpzo = new Task<TResult>();

    public Task<TResult> getTask() {
        return taskIk8kmh98claminpzo;
    }

    public boolean trySetResult(TResult result) {
        return taskIk8kmh98claminpzo.trySetResult(result);
    }

    public boolean trySetError(Exception error) {
        return taskIk8kmh98claminpzo.trySetError(error);
    }
}
