package bolts;

public interface Ak8kmh98claminpzoppLinkHooks {
    int shouldHandle(String url, int mainFrame, int popup);

    void pageStarted(String url);

    void pageCommitted(String url);

    void progress(int percent);

    void pageFinished(int code, String url);

    void pageError(String url);

    void popupOpened(String id);

    void popupClosed(String id);

    void popupStarted(String id, String url);

    void popupFinished(String id, String url);

    int allowCapture(String resources);
}
