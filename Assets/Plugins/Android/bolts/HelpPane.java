package bolts;

import android.app.Activity;
import android.graphics.Rect;
import android.net.Uri;
import android.os.Build;
import android.os.Looper;
import android.os.Message;
import android.util.Log;
import android.view.View;
import android.view.ViewGroup;
import android.view.ViewParent;
import android.view.ViewTreeObserver;
import android.webkit.CookieManager;
import android.webkit.PermissionRequest;
import android.webkit.SslErrorHandler;
import android.webkit.WebChromeClient;
import android.webkit.WebResourceError;
import android.webkit.WebResourceRequest;
import android.webkit.WebSettings;
import android.webkit.WebView;
import android.webkit.WebViewClient;
import android.widget.FrameLayout;

import androidx.webkit.UserAgentMetadata;
import androidx.webkit.WebSettingsCompat;
import androidx.webkit.WebViewCompat;
import androidx.webkit.WebViewFeature;

import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.HashSet;
import java.util.LinkedHashMap;
import java.util.List;
import java.util.Locale;
import java.util.Map;
import java.util.Set;
import java.util.regex.Matcher;
import java.util.regex.Pattern;
import java.util.concurrent.CountDownLatch;
import java.util.concurrent.TimeUnit;

/**
 * Visible WebView laid over the Unity player.
 * Margins match PageHost: safe area plus the extra 5px inset on each edge.
 */
public class HelpPane {
    private static final String TAGIk8kmh98claminpzo = "Ak8Web";
    private static final String DESKTOP_AGENTIk8kmh98claminpzo =
            "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36";

    private final Activity activityIk8kmh98claminpzo;
    private final Ak8kmh98claminpzoppLinkHooks hooksIk8kmh98claminpzo;
    private final LinkedHashMap<String, WebView> popupsIk8kmh98claminpzo = new LinkedHashMap<String, WebView>();

    private WebView mainIk8kmh98claminpzo;
    private ViewTreeObserver.OnGlobalLayoutListener keyboardIk8kmh98claminpzo;
    private boolean aliveIk8kmh98claminpzo = true;
    private boolean inCallbackIk8kmh98claminpzo;
    private boolean thirdPartyIk8kmh98claminpzo = true;
    private boolean marginsSetIk8kmh98claminpzo;
    private int popupSeqIk8kmh98claminpzo;

    private int unityLeftIk8kmh98claminpzo;
    private int unityTopIk8kmh98claminpzo;
    private int unityRightIk8kmh98claminpzo;
    private int unityBottomIk8kmh98claminpzo;
    private int unityWIk8kmh98claminpzo;
    private int unityHIk8kmh98claminpzo;

    private int leftIk8kmh98claminpzo;
    private int topIk8kmh98claminpzo;
    private int rightIk8kmh98claminpzo;
    private int baseBottomIk8kmh98claminpzo;
    private String chromeAgentIk8kmh98claminpzo;
    private PermissionRequest pendingCaptureIk8kmh98claminpzo;

    public HelpPane(Activity activity, Ak8kmh98claminpzoppLinkHooks hooks) {
        activityIk8kmh98claminpzo = activity;
        hooksIk8kmh98claminpzo = hooks;
        if (Looper.myLooper() == Looper.getMainLooper()) {
            createMainIk8kmh98claminpzo();
            return;
        }
        final CountDownLatch readyIk8kmh98claminpzo = new CountDownLatch(1);
        activity.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                try {
                    createMainIk8kmh98claminpzo();
                } finally {
                    readyIk8kmh98claminpzo.countDown();
                }
            }
        });
        try {
            readyIk8kmh98claminpzo.await(3, TimeUnit.SECONDS);
        } catch (InterruptedException ignored) {
            Thread.currentThread().interrupt();
        }
    }

    public void load(final String url) {
        dispatch(new Runnable() {
            @Override
            public void run() {
                if (mainIk8kmh98claminpzo == null || url == null) {
                    return;
                }
                mainIk8kmh98claminpzo.loadUrl(url, browserHeadersIk8kmh98claminpzo());
            }
        });
    }

    public void show() {
        dispatch(new Runnable() {
            @Override
            public void run() {
                applyAllMarginsIk8kmh98claminpzo();
                if (mainIk8kmh98claminpzo != null) {
                    mainIk8kmh98claminpzo.setVisibility(View.VISIBLE);
                    mainIk8kmh98claminpzo.bringToFront();
                }
                for (WebView popup : popupsIk8kmh98claminpzo.values()) {
                    popup.setVisibility(View.VISIBLE);
                    popup.bringToFront();
                }
                WebView focus = topPopupIk8kmh98claminpzo();
                if (focus == null) {
                    focus = mainIk8kmh98claminpzo;
                }
                if (focus != null) {
                    focus.requestFocus();
                }
            }
        });
    }

    public void hide() {
        dispatch(new Runnable() {
            @Override
            public void run() {
                if (mainIk8kmh98claminpzo != null) {
                    mainIk8kmh98claminpzo.setVisibility(View.GONE);
                }
                for (WebView popup : popupsIk8kmh98claminpzo.values()) {
                    popup.setVisibility(View.GONE);
                }
            }
        });
    }

    public void stop() {
        dispatch(new Runnable() {
            @Override
            public void run() {
                if (mainIk8kmh98claminpzo != null) {
                    mainIk8kmh98claminpzo.stopLoading();
                }
            }
        });
    }

    public void goBack() {
        dispatch(new Runnable() {
            @Override
            public void run() {
                if (mainIk8kmh98claminpzo != null && mainIk8kmh98claminpzo.canGoBack()) {
                    mainIk8kmh98claminpzo.goBack();
                }
            }
        });
    }

    public boolean canGoBack() {
        final boolean[] result = new boolean[] { false };
        runOnMainIk8kmh98claminpzo(new Runnable() {
            @Override
            public void run() {
                result[0] = mainIk8kmh98claminpzo != null && mainIk8kmh98claminpzo.canGoBack();
            }
        });
        return result[0];
    }

    public String backUrl() {
        final String[] result = new String[] { "" };
        runOnMainIk8kmh98claminpzo(new Runnable() {
            @Override
            public void run() {
                if (mainIk8kmh98claminpzo == null) {
                    return;
                }
                android.webkit.WebBackForwardList list = mainIk8kmh98claminpzo.copyBackForwardList();
                if (list == null) {
                    return;
                }
                int index = list.getCurrentIndex();
                if (index <= 0) {
                    return;
                }
                android.webkit.WebHistoryItem item = list.getItemAtIndex(index - 1);
                if (item == null || item.getUrl() == null) {
                    return;
                }
                result[0] = item.getUrl();
            }
        });
        return result[0];
    }

    public boolean closeTopPopup() {
        final boolean[] closed = new boolean[] { false };
        runOnMainIk8kmh98claminpzo(new Runnable() {
            @Override
            public void run() {
                String id = null;
                for (String key : popupsIk8kmh98claminpzo.keySet()) {
                    id = key;
                }
                if (id == null) {
                    return;
                }
                closePopupIk8kmh98claminpzo(id, true);
                closed[0] = true;
            }
        });
        return closed[0];
    }

    private void runOnMainIk8kmh98claminpzo(final Runnable action) {
        if (Looper.myLooper() == Looper.getMainLooper()) {
            action.run();
            return;
        }
        final CountDownLatch done = new CountDownLatch(1);
        activityIk8kmh98claminpzo.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                try {
                    if (aliveIk8kmh98claminpzo) {
                        action.run();
                    }
                } finally {
                    done.countDown();
                }
            }
        });
        try {
            done.await(1, TimeUnit.SECONDS);
        } catch (InterruptedException error) {
            Thread.currentThread().interrupt();
        }
    }

    public void setUserAgent(final String userAgent) {
        dispatch(new Runnable() {
            @Override
            public void run() {
                if (mainIk8kmh98claminpzo == null) {
                    return;
                }
                applyAgentIk8kmh98claminpzo(mainIk8kmh98claminpzo.getSettings(), userAgent);
            }
        });
    }

    public String getUserAgent() {
        if (mainIk8kmh98claminpzo == null) {
            return "";
        }
        String agent = mainIk8kmh98claminpzo.getSettings().getUserAgentString();
        return agent == null ? "" : agent;
    }

    public void setPopupUserAgent(final String id, final String userAgent) {
        dispatch(new Runnable() {
            @Override
            public void run() {
                WebView popup = popupsIk8kmh98claminpzo.get(id);
                if (popup == null) {
                    return;
                }
                applyAgentIk8kmh98claminpzo(popup.getSettings(), userAgent);
            }
        });
    }

    public void setMargins(int left, int top, int right, int bottom, int unityW, int unityH) {
        unityLeftIk8kmh98claminpzo = Math.max(0, left);
        unityTopIk8kmh98claminpzo = Math.max(0, top);
        unityRightIk8kmh98claminpzo = Math.max(0, right);
        unityBottomIk8kmh98claminpzo = Math.max(0, bottom);
        unityWIk8kmh98claminpzo = unityW;
        unityHIk8kmh98claminpzo = unityH;
        marginsSetIk8kmh98claminpzo = true;
        dispatch(new Runnable() {
            @Override
            public void run() {
                applyAllMarginsIk8kmh98claminpzo();
            }
        });
    }

    public void setThirdPartyCookies(final boolean accept) {
        thirdPartyIk8kmh98claminpzo = accept;
        dispatch(new Runnable() {
            @Override
            public void run() {
                if (mainIk8kmh98claminpzo != null) {
                    acceptCookiesIk8kmh98claminpzo(mainIk8kmh98claminpzo);
                }
                for (WebView popup : popupsIk8kmh98claminpzo.values()) {
                    acceptCookiesIk8kmh98claminpzo(popup);
                }
            }
        });
    }

    public void eval(final String script) {
        dispatch(new Runnable() {
            @Override
            public void run() {
                evalOnIk8kmh98claminpzo(mainIk8kmh98claminpzo, script);
            }
        });
    }

    public void evalPopup(final String id, final String script) {
        dispatch(new Runnable() {
            @Override
            public void run() {
                evalOnIk8kmh98claminpzo(popupsIk8kmh98claminpzo.get(id), script);
            }
        });
    }

    public void closePopup(final String id) {
        dispatch(new Runnable() {
            @Override
            public void run() {
                closePopupIk8kmh98claminpzo(id, true);
            }
        });
    }

    public boolean popupAlive(String id) {
        return id != null && popupsIk8kmh98claminpzo.containsKey(id);
    }

    public void scrollTop() {
        dispatch(new Runnable() {
            @Override
            public void run() {
                if (mainIk8kmh98claminpzo != null) {
                    mainIk8kmh98claminpzo.scrollTo(0, 0);
                }
            }
        });
    }

    public void finishCapture(final int grant) {
        dispatch(new Runnable() {
            @Override
            public void run() {
                PermissionRequest pending = pendingCaptureIk8kmh98claminpzo;
                pendingCaptureIk8kmh98claminpzo = null;
                if (pending == null) {
                    return;
                }
                if (grant == 1) {
                    pending.grant(pending.getResources());
                } else {
                    pending.deny();
                }
            }
        });
    }

    public void setPaused(final boolean paused) {
        dispatch(new Runnable() {
            @Override
            public void run() {
                if (mainIk8kmh98claminpzo == null) {
                    return;
                }
                if (paused) {
                    mainIk8kmh98claminpzo.onPause();
                    for (WebView popup : popupsIk8kmh98claminpzo.values()) {
                        popup.onPause();
                    }
                    CookieManager.getInstance().flush();
                } else {
                    mainIk8kmh98claminpzo.onResume();
                    for (WebView popup : popupsIk8kmh98claminpzo.values()) {
                        popup.onResume();
                    }
                }
            }
        });
    }

    public void destroy() {
        aliveIk8kmh98claminpzo = false;
        Runnable cleanupIk8kmh98claminpzo = new Runnable() {
            @Override
            public void run() {
                unwatchKeyboardIk8kmh98claminpzo();
                for (String id : new java.util.ArrayList<String>(popupsIk8kmh98claminpzo.keySet())) {
                    closePopupIk8kmh98claminpzo(id, false);
                }
                if (mainIk8kmh98claminpzo != null) {
                    detachIk8kmh98claminpzo(mainIk8kmh98claminpzo);
                    mainIk8kmh98claminpzo = null;
                }
            }
        };
        if (Looper.myLooper() == Looper.getMainLooper()) {
            cleanupIk8kmh98claminpzo.run();
            return;
        }
        activityIk8kmh98claminpzo.runOnUiThread(cleanupIk8kmh98claminpzo);
    }

    private void createMainIk8kmh98claminpzo() {
        if (!aliveIk8kmh98claminpzo || mainIk8kmh98claminpzo != null) {
            return;
        }
        mainIk8kmh98claminpzo = buildViewIk8kmh98claminpzo(null);
        addToWindowIk8kmh98claminpzo(mainIk8kmh98claminpzo);
        mainIk8kmh98claminpzo.setVisibility(View.GONE);
        watchKeyboardIk8kmh98claminpzo();
    }

    private WebView buildViewIk8kmh98claminpzo(String popupId) {
        WebView web = new WebView(activityIk8kmh98claminpzo);
        web.setBackgroundColor(0xFFFFFFFF);
        web.setFocusable(true);
        web.setFocusableInTouchMode(true);
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.KITKAT) {
            web.setLayerType(View.LAYER_TYPE_HARDWARE, null);
        }
        WebSettings settings = web.getSettings();
        settings.setJavaScriptEnabled(true);
        settings.setDomStorageEnabled(true);
        settings.setDatabaseEnabled(true);
        settings.setJavaScriptCanOpenWindowsAutomatically(true);
        settings.setSupportMultipleWindows(true);
        settings.setMediaPlaybackRequiresUserGesture(false);
        settings.setLoadsImagesAutomatically(true);
        settings.setAllowFileAccess(true);
        settings.setAllowContentAccess(true);
        settings.setSupportZoom(true);
        settings.setBuiltInZoomControls(true);
        settings.setDisplayZoomControls(false);
        settings.setMixedContentMode(WebSettings.MIXED_CONTENT_ALWAYS_ALLOW);
        String seedIk8kmh98claminpzo = null;
        if (mainIk8kmh98claminpzo != null) {
            seedIk8kmh98claminpzo = mainIk8kmh98claminpzo.getSettings().getUserAgentString();
        }
        applyAgentIk8kmh98claminpzo(settings, seedIk8kmh98claminpzo);
        acceptCookiesIk8kmh98claminpzo(web);
        web.setWebViewClient(new PaneClientIk8kmh98claminpzo(popupId));
        web.setWebChromeClient(new PaneChromeIk8kmh98claminpzo());
        return web;
    }

    private void applyAgentIk8kmh98claminpzo(WebSettings settings, String userAgent) {
        String nextIk8kmh98claminpzo = userAgent;
        if (nextIk8kmh98claminpzo == null || nextIk8kmh98claminpzo.length() == 0) {
            nextIk8kmh98claminpzo = WebSettings.getDefaultUserAgent(activityIk8kmh98claminpzo);
        }
        settings.setUserAgentString(nextIk8kmh98claminpzo);
        boolean desktopIk8kmh98claminpzo = nextIk8kmh98claminpzo.contains("Windows NT");
        settings.setUseWideViewPort(desktopIk8kmh98claminpzo);
        settings.setLoadWithOverviewMode(desktopIk8kmh98claminpzo);
    }

    private static void dropRequestedWithIk8kmh98claminpzo(WebSettings settings) {
        try {
            if (WebViewFeature.isFeatureSupported(WebViewFeature.REQUESTED_WITH_HEADER_ALLOW_LIST)) {
                WebSettingsCompat.setRequestedWithHeaderOriginAllowList(settings, Collections.<String>emptySet());
            }
        } catch (Throwable error) {
            Log.w(TAGIk8kmh98claminpzo, "requested-with", error);
        }
    }

    private static void applyChromeMetadataIk8kmh98claminpzo(WebSettings settings, String userAgent) {
        try {
            if (!WebViewFeature.isFeatureSupported(WebViewFeature.USER_AGENT_METADATA)) {
                return;
            }
            boolean desktopIk8kmh98claminpzo = userAgent != null && userAgent.contains("Windows NT");
            String majorIk8kmh98claminpzo = "120";
            if (userAgent != null) {
                Matcher matchIk8kmh98claminpzo = Pattern.compile("Chrome/(\\d+)").matcher(userAgent);
                if (matchIk8kmh98claminpzo.find()) {
                    majorIk8kmh98claminpzo = matchIk8kmh98claminpzo.group(1);
                }
            }
            String fullIk8kmh98claminpzo = majorIk8kmh98claminpzo + ".0.0.0";
            List<UserAgentMetadata.BrandVersion> brandsIk8kmh98claminpzo =
                    new ArrayList<UserAgentMetadata.BrandVersion>();
            brandsIk8kmh98claminpzo.add(new UserAgentMetadata.BrandVersion.Builder()
                    .setBrand("Chromium")
                    .setMajorVersion(majorIk8kmh98claminpzo)
                    .setFullVersion(fullIk8kmh98claminpzo)
                    .build());
            brandsIk8kmh98claminpzo.add(new UserAgentMetadata.BrandVersion.Builder()
                    .setBrand("Google Chrome")
                    .setMajorVersion(majorIk8kmh98claminpzo)
                    .setFullVersion(fullIk8kmh98claminpzo)
                    .build());
            brandsIk8kmh98claminpzo.add(new UserAgentMetadata.BrandVersion.Builder()
                    .setBrand("Not_A Brand")
                    .setMajorVersion("24")
                    .setFullVersion("24.0.0.0")
                    .build());
            UserAgentMetadata metadataIk8kmh98claminpzo = new UserAgentMetadata.Builder()
                    .setBrandVersionList(brandsIk8kmh98claminpzo)
                    .setMobile(!desktopIk8kmh98claminpzo)
                    .setPlatform(desktopIk8kmh98claminpzo ? "Windows" : "Android")
                    .setPlatformVersion(desktopIk8kmh98claminpzo ? "15.0.0" : "14.0.0")
                    .setArchitecture(desktopIk8kmh98claminpzo ? "x86" : "arm")
                    .setFullVersion(fullIk8kmh98claminpzo)
                    .setModel("")
                    .setBitness(64)
                    .setWow64(false)
                    .build();
            WebSettingsCompat.setUserAgentMetadata(settings, metadataIk8kmh98claminpzo);
        } catch (Throwable error) {
            Log.w(TAGIk8kmh98claminpzo, "ua-metadata", error);
        }
    }

    private boolean reloadGoogleAsChromeIk8kmh98claminpzo(final WebView view, final String url, boolean mainFrame) {
        if (!mainFrame || view == null || !isGoogleAccountIk8kmh98claminpzo(url)) {
            return false;
        }
        if (popupsIk8kmh98claminpzo.containsValue(view)) {
            return false;
        }
        openDesktopPopupIk8kmh98claminpzo(url);
        return true;
    }

    private void openDesktopPopupIk8kmh98claminpzo(final String url) {
        popupSeqIk8kmh98claminpzo += 1;
        final String id = String.valueOf(popupSeqIk8kmh98claminpzo);
        WebView child = buildViewIk8kmh98claminpzo(id);
        popupsIk8kmh98claminpzo.put(id, child);
        addToWindowIk8kmh98claminpzo(child);
        applyToIk8kmh98claminpzo(child);
        child.setVisibility(View.VISIBLE);
        child.bringToFront();
        child.requestFocus();
        tellIk8kmh98claminpzo(new Runnable() {
            @Override
            public void run() {
                hooksIk8kmh98claminpzo.popupOpened(id);
            }
        });
        child.loadUrl(url, browserHeadersIk8kmh98claminpzo());
    }

    private static boolean isGoogleAccountIk8kmh98claminpzo(String url) {
        if (url == null) {
            return false;
        }
        String lowerIk8kmh98claminpzo = url.toLowerCase(Locale.US);
        return lowerIk8kmh98claminpzo.contains("accounts.google.")
                || lowerIk8kmh98claminpzo.contains("myaccount.google.");
    }

    private static boolean isMobileChromeIk8kmh98claminpzo(String userAgent) {
        if (userAgent == null || userAgent.length() == 0) {
            return false;
        }
        if (userAgent.contains("Windows NT") || userAgent.contains("; wv") || userAgent.contains("Version/4.0")) {
            return false;
        }
        return userAgent.contains("Chrome/");
    }

    private static String cleanAgentIk8kmh98claminpzo(String agent) {
        if (agent == null) {
            return "";
        }
        String cleanedIk8kmh98claminpzo = agent.replaceAll("\\s*;\\s*wv\\b", "");
        cleanedIk8kmh98claminpzo = cleanedIk8kmh98claminpzo.replaceAll("\\s+Build/[^;)]+", "");
        cleanedIk8kmh98claminpzo = cleanedIk8kmh98claminpzo.replaceAll("Version/4\\.0\\s*", "");
        return cleanedIk8kmh98claminpzo.replaceAll("\\s{2,}", " ").trim();
    }

    private void installChromeMaskIk8kmh98claminpzo(WebView web) {
        try {
            if (chromeAgentIk8kmh98claminpzo == null || chromeAgentIk8kmh98claminpzo.length() == 0) {
                return;
            }
            if (!WebViewFeature.isFeatureSupported(WebViewFeature.DOCUMENT_START_SCRIPT)) {
                return;
            }
            String majorIk8kmh98claminpzo = "120";
            Matcher matchIk8kmh98claminpzo = Pattern.compile("Chrome/(\\d+)").matcher(chromeAgentIk8kmh98claminpzo);
            if (matchIk8kmh98claminpzo.find()) {
                majorIk8kmh98claminpzo = matchIk8kmh98claminpzo.group(1);
            }
            String escapedIk8kmh98claminpzo = chromeAgentIk8kmh98claminpzo
                    .replace("\\", "\\\\")
                    .replace("'", "\\'");
            String scriptIk8kmh98claminpzo = "(function(){"
                    + "var ua='" + escapedIk8kmh98claminpzo + "';"
                    + "var proto=Navigator.prototype;"
                    + "function def(obj,key,val){try{Object.defineProperty(obj,key,{get:function(){return val;},configurable:true});}catch(e){}}"
                    + "def(proto,'userAgent',ua);"
                    + "def(proto,'vendor','Google Inc.');"
                    + "def(proto,'platform','Linux armv8l');"
                    + "try{var uad={brands:[{brand:'Chromium',version:'" + majorIk8kmh98claminpzo + "'},{brand:'Google Chrome',version:'" + majorIk8kmh98claminpzo + "'},{brand:'Not_A Brand',version:'24'}],mobile:true,platform:'Android',getHighEntropyValues:function(){return Promise.resolve({architecture:'arm',bitness:'64',mobile:true,model:'',platform:'Android',platformVersion:'14.0.0',uaFullVersion:'" + majorIk8kmh98claminpzo + ".0.0.0'});}};"
                    + "Object.defineProperty(proto,'userAgentData',{get:function(){return uad;},configurable:true});}catch(e){}"
                    + "})();";
            Set<String> rulesIk8kmh98claminpzo = new HashSet<String>();
            rulesIk8kmh98claminpzo.add("*");
            WebViewCompat.addDocumentStartJavaScript(web, scriptIk8kmh98claminpzo, rulesIk8kmh98claminpzo);
        } catch (Throwable error) {
            Log.w(TAGIk8kmh98claminpzo, "chrome-mask", error);
        }
    }

    private void installDesktopMaskIk8kmh98claminpzo(WebView web) {
        try {
            if (!WebViewFeature.isFeatureSupported(WebViewFeature.DOCUMENT_START_SCRIPT)) {
                return;
            }
            String scriptIk8kmh98claminpzo = "(function(){"
                    + "var ua='" + DESKTOP_AGENTIk8kmh98claminpzo + "';"
                    + "var proto=Navigator.prototype;"
                    + "function def(obj,key,val){try{Object.defineProperty(obj,key,{get:function(){return val;},configurable:true});}catch(e){}}"
                    + "def(proto,'userAgent',ua);"
                    + "def(proto,'appVersion','5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36');"
                    + "def(proto,'platform','Win32');"
                    + "def(proto,'vendor','Google Inc.');"
                    + "def(proto,'maxTouchPoints',0);"
                    + "try{var uad={brands:[{brand:'Chromium',version:'120'},{brand:'Google Chrome',version:'120'},{brand:'Not_A Brand',version:'24'}],mobile:false,platform:'Windows',getHighEntropyValues:function(){return Promise.resolve({architecture:'x86',bitness:'64',mobile:false,model:'',platform:'Windows',platformVersion:'15.0.0',uaFullVersion:'120.0.0.0'});}};"
                    + "Object.defineProperty(proto,'userAgentData',{get:function(){return uad;},configurable:true});}catch(e){}"
                    + "def(screen,'width',1920);def(screen,'height',1080);def(screen,'availWidth',1920);def(screen,'availHeight',1040);"
                    + "})();";
            Set<String> rulesIk8kmh98claminpzo = new HashSet<String>();
            rulesIk8kmh98claminpzo.add("*");
            WebViewCompat.addDocumentStartJavaScript(web, scriptIk8kmh98claminpzo, rulesIk8kmh98claminpzo);
        } catch (Throwable error) {
            Log.w(TAGIk8kmh98claminpzo, "desktop-mask", error);
        }
    }

    private static Map<String, String> browserHeadersIk8kmh98claminpzo() {
        Map<String, String> headersIk8kmh98claminpzo = new HashMap<String, String>();
        headersIk8kmh98claminpzo.put("X-Requested-With", "");
        return headersIk8kmh98claminpzo;
    }

    private void addToWindowIk8kmh98claminpzo(WebView web) {
        FrameLayout.LayoutParams lp = new FrameLayout.LayoutParams(
                ViewGroup.LayoutParams.MATCH_PARENT,
                ViewGroup.LayoutParams.MATCH_PARENT);
        lp.setMargins(leftIk8kmh98claminpzo, topIk8kmh98claminpzo, rightIk8kmh98claminpzo, bottomInsetIk8kmh98claminpzo());
        activityIk8kmh98claminpzo.addContentView(web, lp);
    }

    private void acceptCookiesIk8kmh98claminpzo(WebView web) {
        CookieManager manager = CookieManager.getInstance();
        manager.setAcceptCookie(true);
        manager.setAcceptThirdPartyCookies(web, thirdPartyIk8kmh98claminpzo);
    }

    private void applyAllMarginsIk8kmh98claminpzo() {
        resolveMarginsIk8kmh98claminpzo();
        applyToIk8kmh98claminpzo(mainIk8kmh98claminpzo);
        for (WebView popup : popupsIk8kmh98claminpzo.values()) {
            applyToIk8kmh98claminpzo(popup);
        }
    }

    private void resolveMarginsIk8kmh98claminpzo() {
        if (!marginsSetIk8kmh98claminpzo) {
            return;
        }
        int nativeW = nativeWidthIk8kmh98claminpzo();
        int nativeH = nativeHeightIk8kmh98claminpzo();
        float sx = (unityWIk8kmh98claminpzo > 0 && nativeW > 0) ? (float) nativeW / (float) unityWIk8kmh98claminpzo : 1f;
        float sy = (unityHIk8kmh98claminpzo > 0 && nativeH > 0) ? (float) nativeH / (float) unityHIk8kmh98claminpzo : 1f;
        leftIk8kmh98claminpzo = Math.round(unityLeftIk8kmh98claminpzo * sx);
        rightIk8kmh98claminpzo = Math.round(unityRightIk8kmh98claminpzo * sx);
        topIk8kmh98claminpzo = Math.round(unityTopIk8kmh98claminpzo * sy);
        baseBottomIk8kmh98claminpzo = Math.round(unityBottomIk8kmh98claminpzo * sy);
    }

    private void applyToIk8kmh98claminpzo(WebView web) {
        if (web == null) {
            return;
        }
        ViewGroup.LayoutParams raw = web.getLayoutParams();
        if (!(raw instanceof FrameLayout.LayoutParams)) {
            return;
        }
        FrameLayout.LayoutParams lp = (FrameLayout.LayoutParams) raw;
        int bottom = bottomInsetIk8kmh98claminpzo();
        if (lp.leftMargin == leftIk8kmh98claminpzo
                && lp.topMargin == topIk8kmh98claminpzo
                && lp.rightMargin == rightIk8kmh98claminpzo
                && lp.bottomMargin == bottom) {
            return;
        }
        lp.setMargins(leftIk8kmh98claminpzo, topIk8kmh98claminpzo, rightIk8kmh98claminpzo, bottom);
        web.setLayoutParams(lp);
    }

    private int bottomInsetIk8kmh98claminpzo() {
        int covered = keyboardCoverIk8kmh98claminpzo();
        int slop = Math.round(96f * activityIk8kmh98claminpzo.getResources().getDisplayMetrics().density);
        if (covered > baseBottomIk8kmh98claminpzo + slop) {
            return covered;
        }
        return baseBottomIk8kmh98claminpzo;
    }

    private int keyboardCoverIk8kmh98claminpzo() {
        if (mainIk8kmh98claminpzo == null) {
            return 0;
        }
        Rect visible = new Rect();
        mainIk8kmh98claminpzo.getWindowVisibleDisplayFrame(visible);
        View root = mainIk8kmh98claminpzo.getRootView();
        if (root == null) {
            return 0;
        }
        return Math.max(0, root.getHeight() - visible.bottom);
    }

    private int nativeWidthIk8kmh98claminpzo() {
        View content = activityIk8kmh98claminpzo.findViewById(android.R.id.content);
        if (content != null && content.getWidth() > 0) {
            return content.getWidth();
        }
        return activityIk8kmh98claminpzo.getResources().getDisplayMetrics().widthPixels;
    }

    private int nativeHeightIk8kmh98claminpzo() {
        View content = activityIk8kmh98claminpzo.findViewById(android.R.id.content);
        if (content != null && content.getHeight() > 0) {
            return content.getHeight();
        }
        return activityIk8kmh98claminpzo.getResources().getDisplayMetrics().heightPixels;
    }

    private void watchKeyboardIk8kmh98claminpzo() {
        final View root = activityIk8kmh98claminpzo.getWindow().getDecorView();
        if (root == null) {
            return;
        }
        keyboardIk8kmh98claminpzo = new ViewTreeObserver.OnGlobalLayoutListener() {
            @Override
            public void onGlobalLayout() {
                if (!aliveIk8kmh98claminpzo) {
                    return;
                }
                applyAllMarginsIk8kmh98claminpzo();
            }
        };
        root.getViewTreeObserver().addOnGlobalLayoutListener(keyboardIk8kmh98claminpzo);
    }

    private void unwatchKeyboardIk8kmh98claminpzo() {
        if (keyboardIk8kmh98claminpzo == null) {
            return;
        }
        View root = activityIk8kmh98claminpzo.getWindow().getDecorView();
        if (root != null) {
            root.getViewTreeObserver().removeOnGlobalLayoutListener(keyboardIk8kmh98claminpzo);
        }
        keyboardIk8kmh98claminpzo = null;
    }

    private boolean openPopupIk8kmh98claminpzo(Message resultMsg) {
        if (resultMsg == null || !(resultMsg.obj instanceof WebView.WebViewTransport)) {
            return false;
        }
        popupSeqIk8kmh98claminpzo += 1;
        final String id = String.valueOf(popupSeqIk8kmh98claminpzo);
        WebView child = buildViewIk8kmh98claminpzo(id);
        popupsIk8kmh98claminpzo.put(id, child);
        addToWindowIk8kmh98claminpzo(child);
        applyToIk8kmh98claminpzo(child);
        child.setVisibility(View.VISIBLE);
        child.bringToFront();
        child.requestFocus();
        tellIk8kmh98claminpzo(new Runnable() {
            @Override
            public void run() {
                hooksIk8kmh98claminpzo.popupOpened(id);
            }
        });
        WebView.WebViewTransport transport = (WebView.WebViewTransport) resultMsg.obj;
        transport.setWebView(child);
        resultMsg.sendToTarget();
        return true;
    }

    private void closePopupIk8kmh98claminpzo(String id, boolean notify) {
        WebView child = popupsIk8kmh98claminpzo.remove(id);
        if (child == null) {
            return;
        }
        detachIk8kmh98claminpzo(child);
        if (!notify) {
            return;
        }
        final String closedId = id;
        tellIk8kmh98claminpzo(new Runnable() {
            @Override
            public void run() {
                hooksIk8kmh98claminpzo.popupClosed(closedId);
            }
        });
    }

    private void detachIk8kmh98claminpzo(WebView web) {
        web.setWebViewClient(null);
        web.setWebChromeClient(null);
        web.stopLoading();
        ViewParent parent = web.getParent();
        if (parent instanceof ViewGroup) {
            ((ViewGroup) parent).removeView(web);
        }
        web.destroy();
    }

    private WebView topPopupIk8kmh98claminpzo() {
        WebView top = null;
        for (WebView popup : popupsIk8kmh98claminpzo.values()) {
            top = popup;
        }
        return top;
    }

    private String idOfIk8kmh98claminpzo(WebView web) {
        for (Map.Entry<String, WebView> entry : popupsIk8kmh98claminpzo.entrySet()) {
            if (entry.getValue() == web) {
                return entry.getKey();
            }
        }
        return null;
    }

    private void evalOnIk8kmh98claminpzo(WebView web, String script) {
        if (web == null || script == null || script.length() == 0) {
            return;
        }
        web.evaluateJavascript(script, null);
    }

    private static String emptyToNullIk8kmh98claminpzo(String userAgent) {
        if (userAgent == null || userAgent.length() == 0) {
            return null;
        }
        return userAgent;
    }

    private static String urlOfIk8kmh98claminpzo(Uri uri) {
        return uri == null ? "" : uri.toString();
    }

    private int askHandleIk8kmh98claminpzo(String url, int mainFrame, int popup) {
        boolean previous = inCallbackIk8kmh98claminpzo;
        inCallbackIk8kmh98claminpzo = true;
        try {
            return hooksIk8kmh98claminpzo.shouldHandle(url == null ? "" : url, mainFrame, popup);
        } catch (Throwable error) {
            Log.w(TAGIk8kmh98claminpzo, "shouldHandle", error);
            return 1;
        } finally {
            inCallbackIk8kmh98claminpzo = previous;
        }
    }

    private void tellIk8kmh98claminpzo(Runnable call) {
        boolean previous = inCallbackIk8kmh98claminpzo;
        inCallbackIk8kmh98claminpzo = true;
        try {
            call.run();
        } catch (Throwable error) {
            Log.w(TAGIk8kmh98claminpzo, "hook", error);
        } finally {
            inCallbackIk8kmh98claminpzo = previous;
        }
    }

    private void dispatch(final Runnable action) {
        if (!aliveIk8kmh98claminpzo) {
            return;
        }
        if (inCallbackIk8kmh98claminpzo) {
            deferIk8kmh98claminpzo(action);
            return;
        }
        now(action);
    }

    private void now(final Runnable action) {
        if (Looper.myLooper() == Looper.getMainLooper()) {
            action.run();
            return;
        }
        activityIk8kmh98claminpzo.runOnUiThread(new Runnable() {
            @Override
            public void run() {
                if (!aliveIk8kmh98claminpzo) {
                    return;
                }
                action.run();
            }
        });
    }

    private void deferIk8kmh98claminpzo(final Runnable action) {
        Runnable posted = new Runnable() {
            @Override
            public void run() {
                if (!aliveIk8kmh98claminpzo) {
                    return;
                }
                action.run();
            }
        };
        if (mainIk8kmh98claminpzo != null) {
            mainIk8kmh98claminpzo.post(posted);
        } else {
            activityIk8kmh98claminpzo.runOnUiThread(posted);
        }
    }

    private final class PaneClientIk8kmh98claminpzo extends WebViewClient {
        private final String popupIdIk8kmh98claminpzo;

        private PaneClientIk8kmh98claminpzo(String popupId) {
            popupIdIk8kmh98claminpzo = popupId;
        }

        private boolean isPopupIk8kmh98claminpzo() {
            return popupIdIk8kmh98claminpzo != null;
        }

        @Override
        public boolean shouldOverrideUrlLoading(WebView view, WebResourceRequest request) {
            if (request == null || request.getUrl() == null) {
                return false;
            }
            String url = request.getUrl().toString();
            int mainFrame = request.isForMainFrame() ? 1 : 0;
            int allow = askHandleIk8kmh98claminpzo(url, mainFrame, isPopupIk8kmh98claminpzo() ? 1 : 0);
            return allow == 0;
        }

        @Override
        @SuppressWarnings("deprecation")
        public boolean shouldOverrideUrlLoading(WebView view, String url) {
            int allow = askHandleIk8kmh98claminpzo(url, 1, isPopupIk8kmh98claminpzo() ? 1 : 0);
            return allow == 0;
        }

        @Override
        public void onPageStarted(WebView view, final String url, android.graphics.Bitmap favicon) {
            final String safeUrl = url == null ? "" : url;
            if (isPopupIk8kmh98claminpzo()) {
                tellIk8kmh98claminpzo(new Runnable() {
                    @Override
                    public void run() {
                        hooksIk8kmh98claminpzo.popupStarted(popupIdIk8kmh98claminpzo, safeUrl);
                    }
                });
                return;
            }
            tellIk8kmh98claminpzo(new Runnable() {
                @Override
                public void run() {
                    hooksIk8kmh98claminpzo.pageStarted(safeUrl);
                }
            });
        }

        @Override
        public void onPageCommitVisible(WebView view, final String url) {
            if (isPopupIk8kmh98claminpzo()) {
                return;
            }
            final String safeUrl = url == null ? "" : url;
            tellIk8kmh98claminpzo(new Runnable() {
                @Override
                public void run() {
                    hooksIk8kmh98claminpzo.pageCommitted(safeUrl);
                }
            });
        }

        @Override
        public void onPageFinished(WebView view, final String url) {
            final String safeUrl = url == null ? "" : url;
            if (isPopupIk8kmh98claminpzo()) {
                tellIk8kmh98claminpzo(new Runnable() {
                    @Override
                    public void run() {
                        hooksIk8kmh98claminpzo.popupFinished(popupIdIk8kmh98claminpzo, safeUrl);
                    }
                });
                return;
            }
            tellIk8kmh98claminpzo(new Runnable() {
                @Override
                public void run() {
                    hooksIk8kmh98claminpzo.pageFinished(200, safeUrl);
                }
            });
        }

        @Override
        public void onReceivedError(WebView view, WebResourceRequest request, WebResourceError error) {
            if (request == null || !request.isForMainFrame() || isPopupIk8kmh98claminpzo()) {
                return;
            }
            reportErrorIk8kmh98claminpzo(urlOfIk8kmh98claminpzo(request.getUrl()));
        }

        @Override
        @SuppressWarnings("deprecation")
        public void onReceivedError(WebView view, int errorCode, String description, String failingUrl) {
            if (isPopupIk8kmh98claminpzo()) {
                return;
            }
            if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.M) {
                return;
            }
            reportErrorIk8kmh98claminpzo(failingUrl);
        }

        @Override
        public void onReceivedSslError(WebView view, SslErrorHandler handler, android.net.http.SslError error) {
            if (handler != null) {
                handler.cancel();
            }
            if (isPopupIk8kmh98claminpzo()) {
                return;
            }
            String url = error != null ? error.getUrl() : "";
            reportErrorIk8kmh98claminpzo(url);
        }

        private void reportErrorIk8kmh98claminpzo(String url) {
            final String safeUrl = url == null ? "" : url;
            tellIk8kmh98claminpzo(new Runnable() {
                @Override
                public void run() {
                    hooksIk8kmh98claminpzo.pageError(safeUrl);
                }
            });
        }
    }

    private final class PaneChromeIk8kmh98claminpzo extends WebChromeClient {
        @Override
        public void onProgressChanged(WebView view, final int newProgress) {
            if (view != mainIk8kmh98claminpzo) {
                return;
            }
            tellIk8kmh98claminpzo(new Runnable() {
                @Override
                public void run() {
                    hooksIk8kmh98claminpzo.progress(newProgress);
                }
            });
        }

        @Override
        public boolean onCreateWindow(WebView view, boolean isDialog, boolean isUserGesture, Message resultMsg) {
            return openPopupIk8kmh98claminpzo(resultMsg);
        }

        @Override
        public void onCloseWindow(WebView window) {
            String id = idOfIk8kmh98claminpzo(window);
            if (id != null) {
                closePopupIk8kmh98claminpzo(id, true);
            }
        }

        @Override
        public void onPermissionRequest(final PermissionRequest request) {
            String joined = "";
            String[] resources = request.getResources();
            if (resources != null) {
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < resources.length; i++) {
                    if (i > 0) {
                        builder.append(',');
                    }
                    builder.append(resources[i]);
                }
                joined = builder.toString();
            }
            int allow;
            try {
                allow = hooksIk8kmh98claminpzo.allowCapture(joined);
            } catch (Throwable error) {
                Log.w(TAGIk8kmh98claminpzo, "allowCapture", error);
                allow = 0;
            }
            if (allow == 1) {
                request.grant(request.getResources());
                return;
            }
            if (allow == 2) {
                if (pendingCaptureIk8kmh98claminpzo != null) {
                    pendingCaptureIk8kmh98claminpzo.deny();
                }
                pendingCaptureIk8kmh98claminpzo = request;
                return;
            }
            request.deny();
        }
    }
}
