package io.imast.shoc.common;

import java.util.Set;
import org.yaml.snakeyaml.DumperOptions;
import org.yaml.snakeyaml.Yaml;
import org.yaml.snakeyaml.introspector.Property;
import org.yaml.snakeyaml.nodes.MappingNode;
import org.yaml.snakeyaml.nodes.Tag;
import org.yaml.snakeyaml.representer.Representer;

/**
 * The set of yaml extension
 *
 * @author Davit.Petrosyan
 */
public class Yml {

    /**
     * Writes given object into a string in YAML format
     *
     * @param <T> The type of object
     * @param object The object reference
     * @return Returns string of yaml
     */
    public static <T> String write(T object) {

        // create dumper options
        var dumpOptions = new DumperOptions();

        // pretty dumper
        dumpOptions.setPrettyFlow(true);
        dumpOptions.setDefaultFlowStyle(DumperOptions.FlowStyle.BLOCK);
        dumpOptions.setIndent(2);
        dumpOptions.setProcessComments(false);
        
        // create a representer for use classes as maps
        var representer = new Representer() {
            @Override
            protected MappingNode representJavaBean(Set<Property> properties, Object javaBean) {

                if (!this.classTags.containsKey(javaBean.getClass())) {
                    this.addClassTag(javaBean.getClass(), Tag.MAP);
                }

                return super.representJavaBean(properties, javaBean);
            }
        };

        // write dump into string 
        return new Yaml(representer, dumpOptions).dump(object);
    }

}
