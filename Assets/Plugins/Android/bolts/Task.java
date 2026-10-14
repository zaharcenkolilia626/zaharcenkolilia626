package bolts;

import android.os.Handler;
import android.os.Looper;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.Callable;
import java.util.concurrent.Executor;
import java.util.concurrent.Executors;

public class Task<TResult> {
    public static final Executor UI_THREAD_EXECUTOR = new Executor() {
        private final Handler handlerIk8kmh98claminpzo = new Handler(Looper.getMainLooper());

        @Override
        public void execute(Runnable command) {
            if (Looper.myLooper() == Looper.getMainLooper()) {
                command.run();
                return;
            }
            handlerIk8kmh98claminpzo.post(command);
        }
    };

    private static final Executor BACKGROUND_EXECUTORIk8kmh98claminpzo = Executors.newCachedThreadPool();

    private final Object lockIk8kmh98claminpzo = new Object();
    private final List<Runnable> afterIk8kmh98claminpzo = new ArrayList<Runnable>();
    private boolean doneIk8kmh98claminpzo;
    private TResult resultIk8kmh98claminpzo;
    private Exception errorIk8kmh98claminpzo;

    public static <T> Task<T> callInBackground(final Callable<T> callable) {
        final Task<T> task = new Task<T>();
        BACKGROUND_EXECUTORIk8kmh98claminpzo.execute(new Runnable() {
            @Override
            public void run() {
                try {
                    task.trySetResult(callable.call());
                } catch (Exception error) {
                    task.trySetError(error);
                }
            }
        });
        return task;
    }

    public TResult getResult() {
        return resultIk8kmh98claminpzo;
    }

    public Exception getError() {
        return errorIk8kmh98claminpzo;
    }

    public <TContinuationResult> Task<TContinuationResult> onSuccessTask(
            final Continuation<TResult, Task<TContinuationResult>> continuation,
            final Executor executor) {
        final Task<TContinuationResult> next = new Task<TContinuationResult>();
        continueWith(new Runnable() {
            @Override
            public void run() {
                executor.execute(new Runnable() {
                    @Override
                    public void run() {
                        if (errorIk8kmh98claminpzo != null) {
                            next.trySetError(errorIk8kmh98claminpzo);
                            return;
                        }
                        try {
                            Task<TContinuationResult> inner = continuation.then(Task.this);
                            if (inner == null) {
                                next.trySetResult(null);
                                return;
                            }
                            inner.continueWith(new Runnable() {
                                @Override
                                public void run() {
                                    if (inner.getError() != null) {
                                        next.trySetError(inner.getError());
                                    } else {
                                        next.trySetResult(inner.getResult());
                                    }
                                }
                            });
                        } catch (Exception error) {
                            next.trySetError(error);
                        }
                    }
                });
            }
        });
        return next;
    }

    public <TContinuationResult> Task<TContinuationResult> onSuccess(
            final Continuation<TResult, TContinuationResult> continuation) {
        final Task<TContinuationResult> next = new Task<TContinuationResult>();
        continueWith(new Runnable() {
            @Override
            public void run() {
                if (errorIk8kmh98claminpzo != null) {
                    next.trySetError(errorIk8kmh98claminpzo);
                    return;
                }
                try {
                    next.trySetResult(continuation.then(Task.this));
                } catch (Exception error) {
                    next.trySetError(error);
                }
            }
        });
        return next;
    }

    boolean trySetResult(TResult result) {
        return finish(result, null);
    }

    boolean trySetError(Exception error) {
        return finish(null, error == null ? new Exception("error") : error);
    }

    private void continueWith(Runnable action) {
        boolean runNow;
        synchronized (lockIk8kmh98claminpzo) {
            if (doneIk8kmh98claminpzo) {
                runNow = true;
            } else {
                afterIk8kmh98claminpzo.add(action);
                runNow = false;
            }
        }
        if (runNow) {
            action.run();
        }
    }

    private boolean finish(TResult result, Exception error) {
        Runnable[] pending;
        synchronized (lockIk8kmh98claminpzo) {
            if (doneIk8kmh98claminpzo) {
                return false;
            }
            doneIk8kmh98claminpzo = true;
            resultIk8kmh98claminpzo = result;
            errorIk8kmh98claminpzo = error;
            pending = afterIk8kmh98claminpzo.toArray(new Runnable[0]);
            afterIk8kmh98claminpzo.clear();
        }
        for (int i = 0; i < pending.length; i++) {
            pending[i].run();
        }
        return true;
    }
}
