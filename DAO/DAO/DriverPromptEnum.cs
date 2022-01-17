using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DAO
{
    /// <summary>
    /// ユーザーに接続を確立するための確認のメッセージを表示するかどうか、またいつ表示するかを指定します。
    /// </summary>
    public enum DriverPromptEnum
    {
        /// <summary>
        /// 指定された接続文字列に DSN キーワードが含まれている場合、ドライバー マネージャーはその文字列を接続でそのまま使用します。それ以外の場合は、dbDriverPrompt が指定されたときと同じように動作します。
        /// </summary>
        dbDriverComplete = 0,
        /// <summary>
        /// ドライバー マネージャーは指定された接続文字列を接続で使用します。十分な情報が指定されていない場合は、トラップ可能なエラーが返されます。
        /// </summary>
        dbDriverNoPrompt = 1,
        /// <summary>
        /// ドライバー マネージャーは [接続する ODBC データ ソース] ダイアログ ボックスを表示します。接続の確立に使用される接続文字列は、このダイアログ ボックスを通じてユーザーが選択および完成させたデータ ソース名 (DSN) から構築されます。
        /// </summary>
        dbDriverPrompt = 2,
        /// <summary>
        /// (既定値) dbDriverComplete が指定されたときと同じように動作します。ただし、ドライバーは接続を完了するために必要ではないすべての情報のためのコントロールを無効にします。
        /// </summary>
        dbDriverCompleteRequired = 3
    }
}
