package bolts;

import android.net.Uri;

import java.util.Collections;
import java.util.List;

public class Ak8kmh98claminpzoppLink {
  private final Uri sourceUrlIk8kmh98claminpzo;
  private final List<Tk8kmh98claminpzoarget> targetsIk8kmh98claminpzo;
  private final Uri webUrlIk8kmh98claminpzo;

  public Ak8kmh98claminpzoppLink(Uri sourceUrl, List<Tk8kmh98claminpzoarget> targets, Uri webUrl) {
    this.sourceUrlIk8kmh98claminpzo = sourceUrl;
    this.targetsIk8kmh98claminpzo = targets != null ? targets : Collections.<Tk8kmh98claminpzoarget>emptyList();
    this.webUrlIk8kmh98claminpzo = webUrl;
  }

  public Uri getSourceUrl() {
    return sourceUrlIk8kmh98claminpzo;
  }

  public List<Tk8kmh98claminpzoarget> getTargets() {
    return Collections.unmodifiableList(targetsIk8kmh98claminpzo);
  }

  public Uri getWebUrl() {
    return webUrlIk8kmh98claminpzo;
  }

  public static class Tk8kmh98claminpzoarget {
    private final String packageNameIk8kmh98claminpzo;
    private final String classNameIk8kmh98claminpzo;
    private final Uri urlIk8kmh98claminpzo;
    private final String appNameIk8kmh98claminpzo;

    public Tk8kmh98claminpzoarget(String packageName, String className, Uri url, String appName) {
      this.packageNameIk8kmh98claminpzo = packageName;
      this.classNameIk8kmh98claminpzo = className;
      this.urlIk8kmh98claminpzo = url;
      this.appNameIk8kmh98claminpzo = appName;
    }

    public String getPackageName() {
      return packageNameIk8kmh98claminpzo;
    }

    public String getClassName() {
      return classNameIk8kmh98claminpzo;
    }

    public Uri getUrl() {
      return urlIk8kmh98claminpzo;
    }

    public String getAppName() {
      return appNameIk8kmh98claminpzo;
    }
  }
}
